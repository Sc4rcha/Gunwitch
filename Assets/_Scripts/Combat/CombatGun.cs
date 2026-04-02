using GameInfo;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CombatGun : MonoBehaviour
{
    [Header ("Gun Stats")]
    public SOInventoryItemBullet BulletDefault;
    public int MagazineSize;
    public float ShootingCooldown;
    public LayerMask CollisionLayer;
    public float CritMultiplier;

    [Header ("References Scene")]
    public CombatDrum Drum;
    public Camera CombatCamera;
    public RectTransform ShootingArea;
    [Space]
    public GameObject DrumButtonsHolder;
    public Button ButtonUnload;
    public Button ButtonLoadDefault;

    // bullets in gun magazine
    private Bullet[] bullets;

    // Gun states
    public enum GunState {Shooting, Locked, Reloading, Cooldown }
    private GunState State;

    private Collider2D agentCollider;
    private int bulletIndex;

    private CombatPlayer player;

    public void Setup(CombatPlayer player) 
    {
        this.player = player;

        // isntantiate bullets array to match magazine size
        bullets = new Bullet[MagazineSize];

        // add shooting to input actions
        InputSystem.actions.FindAction("Shoot").performed += InputShoot;

        // setup load default bullet button
        ButtonLoadDefault.GetComponent<LoadDefaultInfo>().Setup(BulletDefault.GetBullet());

        // setup drum
        Drum.Setup(MagazineSize);
    }


    public void InputShoot(InputAction.CallbackContext context) 
    {
        // stop if gun cannot shoot
        if (State == GunState.Shooting)
            Shoot();
    }
    public void Shoot() 
    {
        // if cursor outside of shooting area don't shoot
        if (!RectTransformUtility.RectangleContainsScreenPoint(ShootingArea, Mouse.current.position.ReadValue(), CombatCamera))
            return;

        // play shoot animation
        ManagerPlayer.Instance.HUD.Shoot(ShootingCooldown);

        // detect combat agents on crosshair
        agentCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), CollisionLayer);
        if (agentCollider != null)
        {
            // try to get an agent collider to deal damage
            if (agentCollider.GetComponent<CombatAgentCollider>() is CombatAgentCollider collider)
            {
                if (collider != null)
                {
                    // do damage
                    if (collider.IsCrit)
                        collider.Damage((int)(bullets[bulletIndex].Damage * CritMultiplier));
                    else
                        collider.Damage((bullets[bulletIndex].Damage));
                }
            }
        }

        // once player has fired deactivate drum buttons, cannot unload anymore
        DrumButtonsHolder.SetActive(false);

        // start gun cooldown
        StartCoroutine(ShootCooldown());
    }


    public void ChangeState(GunState stateNew) 
    {
        State = stateNew;
    }

    #region Reload
    public void ReloadStart() 
    {
        // set state to reloading
        ChangeState(GunState.Reloading);

        // activate Load and unload buttons
        DrumButtonsHolder.SetActive(true);
        // lock unload button (no bullets to unload)
        ButtonUnload.interactable = false;
        // unlock load default
        ButtonLoadDefault.interactable = true;

        // send reload start to Drum
        Drum.ReloadStart();

        // reset bullet index for loading
        bulletIndex = 0;
    }
    public void LoadBulletDefault() 
    {
        LoadBullet(BulletDefault.GetBullet());
    }
    public void LoadBullet(Bullet bullet) 
    {
        // load bullet
        bullets[bulletIndex] = bullet;
        // pay mana
        player.PlayerReference.ManaUse(bullet.ManaCost);

        // unload button interactable when a bullet is loaded
        ButtonUnload.interactable = true;
        // load default button interactable when bullet is not last bullet on magazine
        ButtonLoadDefault.interactable = bulletIndex < MagazineSize - 1;

        // load bullet visuals
        ManagerPlayer.Instance.HUD.ReloadLoadBullet(bullet);

        // Drum visuals load bullet
        Drum.LoadBullet(bulletIndex, bullet);

        bulletIndex++;

        // rotate drum
        Drum.RotateDrum(bulletIndex);

        // finish reaload if magazine fully loaded
        if (bulletIndex == MagazineSize)
            player.ReloadFinish();
    }
    public void UnloadBullet() 
    {
        // if reloading finished because all bullets already loaded go back
        if (State == GunState.Shooting)
        {
            // restart player reload
            player.ReloadRestart();

            // state to reloading (to lock gun from firing)
            ChangeState(GunState.Reloading);

            // set player portrait lo reload again
            ManagerPlayer.Instance.HUD.Reload(BulletDefault.GetBullet());

            // set bullet index to mag max
            bulletIndex = MagazineSize;
        }

        // turn back bullet index
        bulletIndex--;

        // return mana cost of bullet
        player.PlayerReference.ManaRecover(bullets[bulletIndex].ManaCost);

        // remove bullet reference from bullets array
        bullets[bulletIndex] = null;

        // Drum visuals unload bullet
        Drum.UnloadBullet(bulletIndex);
        // rotate drum
        Drum.RotateDrum(bulletIndex);

        // Lock unload if no bullets, otherwise unlock
        ButtonUnload.interactable = bulletIndex != 0;
        // unlock load bullet button
        ButtonLoadDefault.interactable = true;
    }
    public void ReloadFinish() 
    {
        // set state to Shooting
        ChangeState(GunState.Shooting);

        // send reload finish to Drum
        Drum.ReloadFinish();

        // reset bullet index for shooting
        bulletIndex = 0;
    }
    #endregion

    private IEnumerator ShootCooldown()
    {
        // do cooldown
        ChangeState(GunState.Cooldown);
        yield return new WaitForSeconds(ShootingCooldown);
        ChangeState(GunState.Shooting);

        // rotate drum and fire bullet
        Drum.FireBullet(bulletIndex);
        Drum.RotateDrum(bulletIndex + 1);

        // add bullet index and finish player turn if no more bullets on magazine
        bulletIndex++;
        if (bulletIndex == MagazineSize)
            player.TurnFinish();
    }

}
