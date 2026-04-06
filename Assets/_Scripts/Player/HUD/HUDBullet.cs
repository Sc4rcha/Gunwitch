using GameInfo;
using UnityEngine;
using UnityEngine.UI;

public class HUDBullet : HUDItem
{
    public Image RayImage;

    public override void SetItemInfo(InventoryItem item)
    {
        base.SetItemInfo(item);

        RayImage.color = (item as Bullet).BulletColor;
    }
}
