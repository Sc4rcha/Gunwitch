using GymCombat;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GymCombat
{
    public class GymCombatTestPlayer : MonoBehaviour
    {
        public GymCharacterPlayer Player;
        public float GunCooldown;
        public LayerMask GunLayerCollision;
        public GymBullet[] BulletsReference;
        [Space]
        public Sprite CharacterAim;
        public Sprite CharacterShoot;
        public Sprite CharacterReload;

        [Space]
        public Image CharacterImage;
        public SpriteRenderer[] BulletsRenderers;
        public GameObject BulletSelector;
        public Image BarHealth;
        public Image BarMana;

        private GymBullet[] bullets;
        private int bulletsIndex;
        private bool isReloading;
        private bool isOnCooldown;

        private GymCombatTest manager;
        private GymInput gymInput;

        public void Setup(GymCombatTest manager)
        {
            this.manager = manager;

            gymInput = new GymInput();
            gymInput.Enable();

            gymInput.Combat.Shoot.performed += ctx => Shoot();

            // setup bullets
            bullets = new GymBullet[3];
            bulletsIndex = 0;
            foreach (var bullet in BulletsRenderers)
                bullet.color = Color.grey;

            ReloadStart();
        }


        public void Damage(int value)
        {
            Player.HealthChange(-value);
            BarHealth.fillAmount = (float)Player.HealthCurrent / (float)Player.HealthMax;
        }
        public void Heal (int value) 
        {
            Player.HealthChange(value);
            BarHealth.fillAmount = (float)Player.HealthCurrent / (float)Player.HealthMax;
        }
        public void UseMana (int value)
        {
            Player.ManaChange(-value);
            BarMana.fillAmount = (float)Player.ManaCurrent / (float)Player.ManaMax;
        }

        public void Shoot()
        {
            if (isReloading || bulletsIndex == 0 || isOnCooldown)
                return;

            bulletsIndex--;

            if (bullets[bulletsIndex].BulletType == GymBullet.Type.Heal)
            {
                Heal(bullets[bulletsIndex].Value);
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 100, GunLayerCollision);
                if (hit.collider != null)
                {
                    if (bullets[bulletsIndex].BulletType == GymBullet.Type.Normal)
                    {
                        if (hit.collider.name == "Collider Crit")
                            hit.collider.GetComponentInParent<GymCombatTestEnemy>().Damage(bullets[bulletsIndex].Value * 2);
                        else
                            hit.collider.GetComponentInParent<GymCombatTestEnemy>().Damage(bullets[bulletsIndex].Value);
                    }
                    if (bullets[bulletsIndex].BulletType == GymBullet.Type.AOE)
                    {
                        hit.collider.GetComponentInParent<GymCombatTestEnemy>().Damage(bullets[bulletsIndex].Value);
                        foreach (var enemy in manager.Enemies)
                            enemy.Damage(bullets[bulletsIndex].Value);
                    }

                }
            }

            BulletsRenderers[bulletsIndex].color = Color.gray;

            // start Cooldown
            StartCoroutine(ShootCooldown());
        }


        #region RELOAD
        public void ReloadStart()
        {
            if (bulletsIndex != 0 || !manager.IsEnemyRoundEnd)
                return;

            BulletSelector.gameObject.SetActive(true);
            isReloading = true;

            CharacterImage.sprite = CharacterReload;

        }
        public void LoadBullet(int bulletType)
        {
            // not enough mana
            if (BulletsReference[bulletType].Mana > Player.ManaCurrent)
                return;

            UseMana(BulletsReference[bulletType].Mana);

            bullets[2 - bulletsIndex] = BulletsReference[bulletType];
            BulletsRenderers[2 - bulletsIndex].color = bullets[2 - bulletsIndex].BulletColor;


            bulletsIndex++;

            if (bulletsIndex == BulletsRenderers.Length)
                ReloadFinish();

        }
        public void ReloadFinish()
        {
            CharacterImage.sprite = CharacterAim;

            BulletSelector.gameObject.SetActive(false);
            isReloading = false;

            manager.RoundEnemyStart();
        }
        #endregion

        private IEnumerator ShootCooldown()
        {
            CharacterImage.sprite = CharacterShoot;

            isOnCooldown = true;
            yield return new WaitForSeconds(GunCooldown);
            isOnCooldown = false;

            CharacterImage.sprite = CharacterAim;

            if (bulletsIndex == 0)
                ReloadStart();
        }
    }
}