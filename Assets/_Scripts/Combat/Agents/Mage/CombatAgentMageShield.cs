using UnityEngine;

public class CombatAgentMageShield : CombatAgent
{
    [Header ("Shield")]
    public float ShieldSmoothTime;
    public CombatAgentMage Mage { private get; set; }

    private Vector2 smoothDampVelocity;

    private CombatGun playerGun;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        playerGun = manager.Player.Gun;
    }

    public override void PlayerTurnStart()
    {
        base.PlayerTurnStart();

        // activate colliders
        foreach (var colider in References.Colliders)
            colider.gameObject.SetActive(true);

        // setup
        gameObject.SetActive(true);
        Actor.Startcombat();
    }
    protected override void PlayerTurnUpdate()
    {
        base.PlayerTurnUpdate();

        if (playerGun.IsCursorOnShootingArea)
            transform.position = Vector2.SmoothDamp(transform.position, playerGun.Visuals.Crosshair.position, ref smoothDampVelocity, ShieldSmoothTime);
        else
            transform.position = Vector2.SmoothDamp(transform.position, Mage.References.Pivot.position, ref smoothDampVelocity, ShieldSmoothTime);
    }
    public override void PlayerTurnFinish()
    {
        base.PlayerTurnFinish();

        // hide shield
        gameObject.SetActive(false);
    }
}
