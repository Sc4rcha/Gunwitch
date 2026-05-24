using UnityEngine;

public class CombatAgentReferences : MonoBehaviour
{
    public Transform Pivot;
    public Transform MarkerDamage;
    [Space]
    public CombatAgentUI AgentUI;
    public Animator Animator;
    public BulletHitEffect BulletHitEffect;
    public SpriteRenderer Renderer;
    public SpriteRenderer[] RenderersExtra;
    [Space]
    public CombatAgentCollider[] Colliders;
}
