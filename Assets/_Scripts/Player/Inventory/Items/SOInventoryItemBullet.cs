using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Inventory/Bullet")]
public class SOInventoryItemBullet : SOInventoryItem
{
    public int ManaCost;
    public int Damage;
    public bool IsMagic;
    public Color BulletColor;

    public SOStatusEffect StatusEffect;

    private void OnValidate()
    {
        // force type to bullet
        Type = GameInfo.ItemType.BULLET;
    }

    public GameInfo.Bullet GetBullet() 
    {
        return new GameInfo.Bullet(this);
    }

    public virtual void BulletEffect(List <CombatAgentCollider> colliders, Vector2 mousePosition) 
    {
        if (colliders.Count == 0)
            return;

        int damage = 0;
        foreach (var collider in GetColliders(colliders))
        {
            collider.Shoot(GetBullet(), mousePosition, ref damage);

            if (StatusEffect != null)
                collider.Agent.AddStatusEffect(StatusEffect.StatusEffect);
        }

    }
    protected virtual CombatAgentCollider[] GetColliders(List<CombatAgentCollider> colliders) 
    {
        CombatAgentCollider targetCollider = null;

        // sort colliders by prio
        colliders.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        if (colliders.Count != 0)
        {
            // priorize crits of higest priority agent
            targetCollider = colliders[0];
            foreach (var col in colliders)
            {
                if (col.IsCrit && targetCollider.Agent == col.Agent)
                    targetCollider = col;
            }
        }

        CombatAgentCollider[] retunValue = new CombatAgentCollider[1];
        retunValue[0] = targetCollider;

        return retunValue;
    }
}
