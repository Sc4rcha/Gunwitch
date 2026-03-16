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
        public Sprite GunOpen;
        public Sprite GunClose;

        [Space]
        public RectTransform Crosshair;
        public Image CharacterImage;
        public Image Gun;
        public Image[] BulletsRenderers;
        public GameObject BulletSelector;
        public Image BarHealth;
        public Image BarMana;
        public Animator CharacterAnimator;
        public Transform DrumTransform;

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
            bulletsIndex = 3;
            foreach (var bullet in BulletsRenderers)
                bullet.color = Color.grey;

            ReloadStart();
        }


        public void Damage(int value)
        {
            CharacterAnimator.Play("Hurt");

            // start Cooldown
            StartCoroutine(HurtCooldown());

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
            if (isReloading || bulletsIndex == 3 || isOnCooldown)
                return;

            DrumTransform.localRotation = Quaternion.Euler(0, 0, GetDrumRotation(bulletsIndex));

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

            bulletsIndex++;

            CharacterAnimator.Play("Shoot");

            // start Cooldown
            StartCoroutine(ShootCooldown());
        }


        private void Update()
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)Crosshair.parent, Mouse.current.position.ReadValue(), Camera.main, out var p);
            Crosshair.position = p;
        }



        #region RELOAD
        public void ReloadStart()
        {
            if (bulletsIndex != 3 || !manager.IsEnemyRoundEnd)
                return;

            bulletsIndex = 0;

            BulletSelector.gameObject.SetActive(true);
            isReloading = true;

            CharacterAnimator.Play("Reload");
            Gun.sprite = GunOpen;
        }
        public void LoadBullet(int bulletType)
        {
            // not enough mana
            if (BulletsReference[bulletType].Mana > Player.ManaCurrent)
                return;

            UseMana(BulletsReference[bulletType].Mana);

            bullets[bulletsIndex] = BulletsReference[bulletType];
            BulletsRenderers[bulletsIndex].color = bullets[bulletsIndex].BulletColor;

            DrumTransform.localRotation = Quaternion.Euler(0, 0, GetDrumRotation(bulletsIndex + 1));

            bulletsIndex++;

            if (bulletsIndex == BulletsRenderers.Length)
                ReloadFinish();

        }
        public void ReloadFinish()
        {
            bulletsIndex = 0;

            CharacterAnimator.Play("Aim");
            Gun.sprite = GunClose;

            BulletSelector.gameObject.SetActive(false);
            isReloading = false;

            manager.RoundEnemyStart();
        }
        #endregion

        private IEnumerator ShootCooldown()
        {
            DrumTransform.localRotation = Quaternion.Euler(0, 0, GetDrumRotation(bulletsIndex - 1));

            isOnCooldown = true;
            yield return new WaitForSeconds(GunCooldown);
            isOnCooldown = false;

            DrumTransform.localRotation = Quaternion.Euler(0, 0, GetDrumRotation(bulletsIndex));

            if (bulletsIndex == 3)
                ReloadStart();
        }
        private IEnumerator HurtCooldown()
        {
            isOnCooldown = true;
            yield return new WaitForSeconds(GunCooldown);
            isOnCooldown = false;
        }



        float GetDrumRotation(int holeIndex, int chambers = 3)
        {
            float holeSpacing = 360f / chambers;
            return holeIndex * holeSpacing;
        }

    }
}