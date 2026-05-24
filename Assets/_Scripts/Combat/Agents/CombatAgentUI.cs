using GameInfo;
using UnityEngine;

public class CombatAgentUI : MonoBehaviour
{
    public CombatAgentUIStatusEffect[] StatusEffects;
    public Transform StatusHolder;

    public void RefreshStatusEffects(ActorEnemy actor, SOCombatConfig config) 
    {
        foreach (var statusEffect in StatusEffects)
            statusEffect.Hide();

        for (int i = 0; i < actor.StatusEffects.Count; i++)
            StatusEffects[i].Show(config.GetStatusEffectInfo(actor.StatusEffects[i]));
    }
}
