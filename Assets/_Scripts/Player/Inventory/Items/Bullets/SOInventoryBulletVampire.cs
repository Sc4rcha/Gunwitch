using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Vampire", menuName = "Inventory/Bullet Vampire")]
public class SOInventoryBulletVampire : SOInventoryItemBullet
{
    public override void BulletEffect(List<CombatAgentCollider> colliders, Vector2 mousePosition)
    {
        base.BulletEffect(colliders, mousePosition);

        if (colliders.Count == 0)
            return;

        int damage = 0;
        foreach (var collider in GetColliders(colliders))
            collider.Shoot(GetBullet(), mousePosition, ref damage);

        ManagerGameElements.Instance.Player.Heal(damage);
    }
}
