using GameInfo;
using GymCombat;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatGun : MonoBehaviour
{
    [Header ("Gun Stats")]
    public SOBullet BulletBase;
    public int MagazineSize;
    public float ShootingCooldown;
    public LayerMask CollisionLayer;
    public float CritMultiplier;

    [Header ("References Scene")]
    public CombatDrum Drum;

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

        // start gun cooldown
        StartCoroutine(ShootCooldown());
    }


    public void SetState(GunState stateNew) 
    {
        State = stateNew;
    }

    #region Reload
    public void ReloadStart() 
    {
        // set state to reloading
        SetState(GunState.Reloading);

        // reset bullet index for loading
        bulletIndex = 0;
    }
    public void LoadBulletDefault() 
    {
        LoadBullet(BulletBase.GetBullet());
    }
    public void LoadBullet(Bullet bullet) 
    {
        // load default bullet
        bullets[bulletIndex] = bullet;

        // load bullet
        Drum.LoadBullet(bulletIndex);

        bulletIndex++;

        // rotate bullet
        Drum.RotateDrum(bulletIndex);

        // finish reaload if magazine fully loaded
        if (bulletIndex == MagazineSize)
            ReloadFinish();
    }
    public void ReloadFinish() 
    {
        // set state to Shooting
        SetState(GunState.Shooting);

        // reset bullet index for shooting
        bulletIndex = 0;
    }
    #endregion

    private IEnumerator ShootCooldown()
    {
        // do cooldown
        SetState(GunState.Cooldown);
        yield return new WaitForSeconds(ShootingCooldown);
        SetState(GunState.Shooting);

        // rotate drum and fire bullet
        Drum.FireBullet(bulletIndex);
        Drum.RotateDrum(bulletIndex + 1);

        // add bullet index and finish player turn if no more bullets on magazine
        bulletIndex++;
        if (bulletIndex == MagazineSize)
            player.TurnFinish();
    }

}
