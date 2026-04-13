using UnityEngine;

public class CombatAgentMage : CombatAgent
{
    [Header("Mage")]
    public CombatAgentMageShield Shield;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        Shield.Setup(manager);
        Shield.Mage = this;
    }


    public override void PlayerTurnStart()
    {
        base.PlayerTurnStart();

        // shield player turn start (start Movement)
        Shield.PlayerTurnStart();
    }

    public override void PlayerTurnFinish()
    {
        base.PlayerTurnFinish();

        // shield player turn finish (finish Movement)
        Shield.PlayerTurnFinish();
    }
}
