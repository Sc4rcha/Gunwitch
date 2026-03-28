using UnityEngine;

public class CombatAgentCollider : MonoBehaviour
{
    public bool IsCrit;

    private CombatAgent agent;
    public void Setup(CombatAgent agent) 
    {
        this.agent = agent;
    }

    /// <summary>
    /// called by GUN when shooting
    /// </summary>
    /// <param name="value"></param>
    public void Damage(int value) 
    {
        // send damage to agent
        agent.Damage(value);
    }
}
