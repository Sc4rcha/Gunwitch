using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Player/Bullet")]
public class SOInventoryItemBullet : SOInventoryItem
{
    public int ManaCost;
    public int Damage;

    private void OnValidate()
    {
        // force type to bullet
        Type = GameInfo.ItemType.BULLET;
    }

    public GameInfo.Bullet GetBullet() 
    {
        return new GameInfo.Bullet(this);
    }
}
