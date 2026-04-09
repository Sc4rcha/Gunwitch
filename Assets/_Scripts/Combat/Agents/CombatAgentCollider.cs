using UnityEngine;

public class CombatAgentCollider : MonoBehaviour
{
    public bool IsCrit;
    public int Priority { get; private set; }

    private CombatAgent agent;
    public void Setup(CombatAgent agent) 
    {
        this.agent = agent;
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
        agent.Damage(value);
    }
}
