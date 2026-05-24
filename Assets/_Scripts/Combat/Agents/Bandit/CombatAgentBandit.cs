using UnityEngine;

public class CombatAgentBandit : CombatAgent
{
    [Header ("Bandit")]
    public float TimeToChangePosition;
    public float TimeToChangePositionVariance;
    private float timeToChangePositionCurrent;
    [Space]
    public CombatAgentBanditItem Item;

    private bool isStanding;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        // Setup bandit item
        Item.Setup(manager, this);
        manager.Encounter.AddSubEnemyToEncounter(Item);
    }

    #region Player turn
    public override void PlayerTurnStart()
    {
        base.PlayerTurnStart();

        timeToChangePositionCurrent = Random.Range(0, TimeToChangePositionVariance);

        // item player turn start (start Movement)
        Item.PlayerTurnStart();
    }
    public override void PlayerTurnFinish()
    {
        base.PlayerTurnFinish();

        // item player turn finish (finish Movement)
        Item.PlayerTurnFinish();
    }
    protected override void PlayerTurnUpdate()
    {
        base.PlayerTurnUpdate();

        // timer for changing position
        timeToChangePositionCurrent += Time.deltaTime;
        if (TimeToChangePosition < timeToChangePositionCurrent)
        {
            timeToChangePositionCurrent = Random.Range(0, TimeToChangePositionVariance);
            ChangePosition();
        }
    }
    #endregion


    public override void TurnStart()
    {
        if (!Item.Actor.IsDead)
            base.TurnStart();
        else
            TurnFinish();
    }
    protected override SOEnemyAction SelectAction()
    {
        return Item.SelectedAction;
    }


    protected override void Die()
    {
        base.Die();

        Item.gameObject.SetActive(false);
    }

    private void ChangePosition() 
    {
        isStanding = !isStanding;
        References.Animator.SetBool("Standing", isStanding);

        if (isStanding)
            References.Animator.Play("Stand-In");
        else
            References.Animator.Play("Crouch-In");
    }
}
