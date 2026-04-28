using UnityEngine;

public class CombatAgentBanditLeaderSword : CombatAgent
{
    public CombatAgentBanditLeaderSheath[] SheathedSwords;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        // play idle animatio on setup
        Animator.Play("Idle");

        // setup sheathed swords
        foreach (var sword in SheathedSwords)
            sword.Setup(manager);
    }

    protected override void Cleanup()
    {
        base.Cleanup();

        // reactivate sword and play Dead animation instead
        gameObject.SetActive(true);
        Animator.Play("Dead");
    }

    public void Revive()
    {
        // look for available sheathed sword to revive
        foreach (var sword in SheathedSwords)
        {
            if (!sword.Actor.IsDead)
            {
                // revive self
                base.Setup(manager);
                Animator.Play("Idle");

                // kill sheathed sword
                sword.ForceKill();
                break;
            }
        }
    }
}
