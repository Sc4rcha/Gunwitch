using UnityEngine;

public class CombatAgentCollider : MonoBehaviour
{
    public bool IsCrit;
    public int Priority { get; private set; }
    public CombatAgent Agent { get; private set; }

    public void Setup(CombatAgent agent) 
    {
        Agent = agent;
        Priority = agent.Priority;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// called by GUN when shooting
    /// </summary>
    /// <param name="value"></param>
    public void Shoot(GameInfo.Bullet bullet, Vector2 mousePosition, ref int damage) 
    {
        // send damage to agent (order is important dont change)
        Agent.BulletHit(mousePosition);
        Agent.Damage(bullet, IsCrit, ref damage);
    }
}
