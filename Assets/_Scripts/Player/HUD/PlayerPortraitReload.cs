using GameInfo;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPortraitReload : MonoBehaviour
{
    public RectTransform BulletCircleCentre;
    public float BulletCircleRadius = 200f;
    public float BulletCircleElipseStrength;
    public Transform ParentFront;
    public Transform ParentBack;
    public List<PlayerPortraitBullet> Bullets;
    public Animator ReloadAnimator;

    private float rotationSpeed = 5f;
    private float focusAngle = 300f;
    private float rotationOffset = 0f;
    private float targetOffset;

    private int bulletsCount;
    private float bulletsAngleStep;
    private float bulletAngle;
    private float bulletPosX;
    private float bulletPosY;

    public void ReloadStart(Bullet defaultBullet) 
    {
        // Get bullets count = bullets in inventory + 1 for the defalt bullet
        bulletsCount = ManagerGameElements.Instance.Player.Info.Inventory.Bullets.Count + 1;

        // hide all bullets
        foreach (var bullet in Bullets)
            bullet.gameObject.SetActive(false);

        // show bullets in inventory
        Bullets[0].gameObject.SetActive(true);
        Bullets[0].SetBulletInfo(defaultBullet);
        for (int i = 1; i < bulletsCount;i++)
        {
            Bullets[i].gameObject.SetActive(true);
            Bullets[i].SetBulletInfo(ManagerGameElements.Instance.Player.Info.Inventory.Bullets.ElementAt (i-1).Value);
        }

        // get angle step
        bulletsAngleStep = 360f / bulletsCount;
    }
    public void FocusBullet(Bullet bullet)
    {
        float itemBaseAngle = Bullets.FindIndex (x => x.BulletID == bullet.Id) * bulletsAngleStep;

        targetOffset = NormalizeAngle(focusAngle - itemBaseAngle);
    }
    public void LoadBullet(Bullet bullet)
    {
        // get bullet index
        int bulletindex = Bullets.FindIndex(x => x.BulletID == bullet.Id);

        // set circle angle to match selected bullet
        targetOffset = NormalizeAngle(focusAngle - bulletindex * bulletsAngleStep);
        rotationOffset = targetOffset;

        // play load animation for bullet
        Bullets[bulletindex].BulletLoad();
        ReloadAnimator.Play("Load");
    }


    void Update()
    {
        // circle rotation animation
        rotationOffset = Mathf.LerpAngle(rotationOffset, targetOffset, Time.deltaTime * rotationSpeed);

        // update bullet positions
        Arrange();
    }

    void Arrange()
    {
        for (int i = 0; i < bulletsCount; i++)
        {
            // get angle of bullet
            bulletAngle = (i * bulletsAngleStep + rotationOffset) * Mathf.Deg2Rad;

            // get position within circle for bullet angle
            bulletPosX = Mathf.Cos(bulletAngle) * BulletCircleRadius;
            bulletPosY = Mathf.Sin(bulletAngle) * BulletCircleRadius / BulletCircleElipseStrength;

            // place bullet in circle
            Bullets[i].BulletRectTransform.anchoredPosition = BulletCircleCentre.anchoredPosition + new Vector2(bulletPosX, bulletPosY);

            // Determine front/back depending on Y position
            if (bulletPosY > 0)
            {
                // In front of character
                if (Bullets[i].BulletRectTransform.parent != ParentFront)
                    Bullets[i].BulletRectTransform.SetParent(ParentFront);
            }
            else
            {
                // Behind character
                if (Bullets[i].BulletRectTransform.parent != ParentBack)
                    Bullets[i].BulletRectTransform.SetParent(ParentBack);
            }
        }
    }

   
    /// <summary>
    /// returns an angle from 0 to 360
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }
}
