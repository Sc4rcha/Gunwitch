using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Bullet")]
public class SOInventoryItemBullet : SOInventoryItem
{
    public int ManaCost;
    public int Damage;
    public Color BulletColor;

    private void OnValidate()
    {
        // force type to bullet
        Type = GameInfo.ItemType.BULLET;
    }

    public GameInfo.Bullet GetBullet() 
    {
        return new GameInfo.Bullet(this);
    }

    public override void ItemEffect()
    {
        base.ItemEffect();
    }
}
