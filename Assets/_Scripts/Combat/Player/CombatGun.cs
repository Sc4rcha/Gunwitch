using GameInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CombatGun : MonoBehaviour
{
    [Header ("Gun Stats")]
    public int MagazineSize;
    public float ShootingCooldown;
    public LayerMask CollisionLayer;
    public float CritMultiplier;

    [Header ("References Scene")]
    public CombatDrum Drum;
    public Camera CombatCamera;
    public RectTransform ShootingArea;
    public CombatGunVisuals Visuals;
    [Space]
    public GameObject DrumButtonsHolder;
    public Button ButtonUnload;

    // bullets in gun magazine
    private Bullet[] bullets;

    // Gun states
    public bool IsCursorOnShootingArea { get; private set; }
    private bool isCursorOnShootingAreaPrevious;
    private bool isOnCooldown;

    // shooting detected colliders
    private List<CombatAgentCollider> agentColliders;
    private CombatAgentCollider targetCollider;
    private int bulletIndex;


    private CombatPlayer player;

    public void Setup(CombatPlayer player) 
    {
        this.player = player;

        // isntantiate bullets array to match magazine size
        bullets = new Bullet[MagazineSize];
        // instantiate agent collider list
        agentColliders = new List<CombatAgentCollider>();

        // add shooting to input actions
        InputSystem.actions.FindAction("Shoot").performed += InputShoot;

        // setup drum
        Drum.Setup(MagazineSize);
    }


    public void CombatFinish ()
    {
        Visuals.Show(false);
        DrumButtonsHolder.SetActive(false);

        enabled = false;
    }

    private void Update()
    {
        // get if cursor is inside the encounter screen
        isCursorOnShootingAreaPrevious = IsCursorOnShootingArea;
        IsCursorOnShootingArea = RectTransformUtility.RectangleContainsScreenPoint(ShootingArea, Mouse.current.position.ReadValue(), CombatCamera);

        // detect if cursoer entered or exited this turn
        if (IsCursorOnShootingArea != isCursorOnShootingAreaPrevious && !isOnCooldown)
        {
            // change portrait state aim or not aim
            player.AimOnOff();
        }

        // update cursor
        Visuals.Show(IsCursorOnShootingArea && player.State == CombatPlayer.PlayerState.Shooting);
        Visuals.UpdatePosition(CombatCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }

    #region Reload
    public void ReloadStart() 
    {
        // hide Crosshair


        // activate Load and unload buttons
        DrumButtonsHolder.SetActive(true);
        // lock unload button (no bullets to unload)
        ButtonUnload.interactable = false;

        // send reload start to Drum
        Drum.ReloadStart();

        // reset bullet index for loading
        bulletIndex = 0;
    }
    public void LoadBullet(Bullet bullet) 
    {
        // load bullet
        bullets[bulletIndex] = bullet;
        // pay mana
        player.PlayerReference.ManaUse(bullet.ManaCost);

        // unload button interactable when a bullet is loaded
        ButtonUnload.interactable = true;

        // Visuals load bullet
        Drum.LoadBullet(bulletIndex, bullet);
        PlayerHUDPortrait.Instance.ReloadLoad(bullet.Id);

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
        if (player.State != CombatPlayer.PlayerState.Reloading)
        {
            // restart player reload
            player.ReloadRestart();

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
    }
    public void ReloadFinish() 
    {
        // send reload finish to Drum
        Drum.ReloadFinish();
    }
    #endregion

    #region Shoot
    public void InputShoot(InputAction.CallbackContext context)
    {
        // stop if gun cannot shoot
        if (player.State == CombatPlayer.PlayerState.Shooting && !isOnCooldown)
            Shoot();
    }
    public void Shoot()
    {
        // if cursor outside of shooting area don't shoot
        if (!IsCursorOnShootingArea)
            return;

        // until first shoot bulletIndex is set at max in case you want to unload bullets
        if (bulletIndex == MagazineSize)
            bulletIndex = 0;

        // play shoot animation
        Visuals.Shoot();
        PlayerHUDPortrait.Instance.GunShoot();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // detect combat agents on crosshair
        agentColliders.Clear();
        foreach (var col in Physics2D.RaycastAll(mousePosition, Vector2.zero, Mathf.Infinity, CollisionLayer))
        {
            if (col.collider.GetComponent<CombatAgentCollider>() is CombatAgentCollider agentCol)
                agentColliders.Add(agentCol);
        }

        // sort colliders by prio
        agentColliders.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        if (agentColliders.Count != 0)
        {
            // show bullet stendil
            Visuals.ShowStencil(mousePosition);

            // priorize crits
            targetCollider = agentColliders[0];
            foreach (var col in agentColliders)
            {
                if (col.IsCrit)
                    targetCollider = col;
            }

            // do damage
            if (targetCollider.IsCrit)
                targetCollider.Damage((int)(bullets[bulletIndex].Damage * CritMultiplier));
            else
                targetCollider.Damage((bullets[bulletIndex].Damage));
        }

        // once player has fired deactivate drum buttons, cannot unload anymore
        DrumButtonsHolder.SetActive(false);

        // start gun cooldown
        StartCoroutine(ShootCooldown());
    }
    private IEnumerator ShootCooldown()
    {
        // fire bullet
        Drum.FireBullet(bulletIndex);

        // do cooldown
        isOnCooldown = true;
        yield return new WaitForSeconds(ShootingCooldown);
        isOnCooldown = false;

        // rotate drum
        Drum.RotateDrum(bulletIndex + 1);

        // change portrait state aim or not aim
        player.AimOnOff();

        // add bullet index and finish player turn if no more bullets on magazine
        bulletIndex++;
        if (bulletIndex == MagazineSize)
            player.TurnFinish();
    }
    #endregion
}
