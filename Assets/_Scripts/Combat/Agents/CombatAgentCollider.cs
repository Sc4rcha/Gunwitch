using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    public void Damage(int value) 
    {
        // send damage to agent
        Agent.Damage(value, IsCrit);
    }

    public void Shoot(int value, Vector2 mousePosition) 
    {
        // send damage to agent
        Agent.Damage(value, IsCrit);
        Agent.BulletHit(mousePosition);
    }
}
