using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Player/Bullet")]
public class SOBullet : SOInventoryItem
{
    public int ManaCost;
    public int Damage;

    public GameInfo.Bullet GetBullet() 
    {
        return new GameInfo.Bullet(this);
    }
}
