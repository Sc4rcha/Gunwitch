using UnityEngine;
using UnityEngine.UI;

public class PlayerPortraitBullet : MonoBehaviour
{
    public string BulletID;
    public RectTransform BulletRectTransform;
    public Image BulletImage;
    public Image RayImage;
    public Animator BulletAnimator;

    public void SetBulletInfo(GameInfo.Bullet bullet) 
    {
        BulletID = bullet.Id;
        BulletImage.color = bullet.BulletColor;
        RayImage.color = bullet.BulletColor;
    }

    public void BulletLoad() 
    {
        BulletAnimator.Play("Load");
    }
}
