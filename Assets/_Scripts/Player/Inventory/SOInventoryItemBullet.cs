using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Player/Bullet")]
public class SOInventoryItemBullet : SOInventoryItem
{
    public int Damage;

    public GameInfo.Bullet GetBullet() 
    {
        return new GameInfo.Bullet(this);
    }
}
