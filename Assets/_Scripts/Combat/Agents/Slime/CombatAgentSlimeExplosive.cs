using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombatAgentSlimeExplosive : CombatAgentSlime
{
    [Header("SLIME Explosive")]
    public GameObject ExplosionEffect;
    public float ExplosionRadius;
    public SOEnemySkill ExplosionSkill;

    private Collider2D[] detectedColliders;
    private List<CombatAgent> detectedAgents;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        detectedAgents = new List<CombatAgent>();
    }

    protected override void Cleanup()
    {
        base.Cleanup();

        Explode();
    }

    private void Explode() 
    {
        detectedAgents.Clear();
        detectedColliders = Physics2D.OverlapCircleAll(References.Pivot.position, ExplosionRadius);

        // show effect
        Instantiate(ExplosionEffect, References.Pivot.position, Quaternion.identity, manager.Encounter.transform);

        // detect agents inside explosion
        foreach (var colider in detectedColliders)
        {
            if (colider.GetComponent<CombatAgentCollider>() is CombatAgentCollider agentColider)
            {
                if (!detectedAgents.Contains(agentColider.Agent))
                    detectedAgents.Add(agentColider.Agent);
            }
        }

        // damage agents
        foreach (var agent in detectedAgents)
            agent.Damage(Actor, ExplosionSkill);
    }

#if UNITY_EDITOR
    protected override void DrawGizmos()
    {
        base.DrawGizmos();
        Handles.DrawWireDisc(GetComponent<CombatAgentReferences>().Pivot.position, Vector3.forward, ExplosionRadius);
    }
#endif
}
