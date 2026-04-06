using System.Collections;
using GameInfo;
using UnityEngine;

public class CombatAgent : MonoBehaviour
{
    public Actor Actor { get; private set; }

    [Header ("Enemy Info")]
    public SOCombatEnemy EnemyStatsReference;
    public int Priority;

    [Header ("Scene References")]
    public SpriteRenderer Renderer;
    public CombatAgentCollider[] Colliders;
    public Animator Animator;


    protected ManagerCombat manager;

    private bool isPlayerTurn;
    private const float turnCooldown = 1;

    public virtual void Setup(ManagerCombat manager)
    {
        // set references
        this.manager = manager;

        // Setup colliders
        foreach (var collider in Colliders)
            collider.Setup(this);

        // setup agent stats
        Actor = EnemyStatsReference.GetCombatActor();
        Actor.Startcombat();

        Renderer.sortingOrder = Priority * 10;
        isPlayerTurn = false;
    }

    private void Update()
    {
        if (isPlayerTurn && !Actor.IsDead)
            PlayerTurnBehaviour();
    }


    #region Enemy turn
    public virtual void TurnStart()
    {
        // if dead end go to next enemy
        if (Actor.IsDead)
        {
            manager.EnemyTurnFinish();
            return;
        }

        // start turn behaviour coroutine
        StartCoroutine(TurnBehaviour());
    }
    public virtual void TurnFinish()
    {
        // do turn end stuff

        // send turn finish to manager for next enemy to act
        manager.EnemyTurnFinish();
    }
    private IEnumerator TurnBehaviour()
    {
        // initial wait before taking action
        yield return new WaitForSeconds(turnCooldown);

        // enemy takes action
        Act();

        // wait before ending turn
        yield return new WaitForSeconds(turnCooldown);

        // end enemy turn
        TurnFinish();
    }
    protected virtual void Act()
    {
        // visual feedback for acting
        Animator.Play("Attack");

        // do 2 damage to player by default
        manager.Player.PlayerReference.Damage(2);
    }
    #endregion


    #region Player turn
    public virtual void PlayerTurnStart()
    {
        isPlayerTurn = true;
    }
    public virtual void PlayerTurnFinish()
    {
        isPlayerTurn = false;
    }
    protected virtual void PlayerTurnBehaviour() { }
    #endregion


    #region Stat change
    public virtual void Damage(int value)
    {
        Actor.HealthChange(-value);

        // visual feedback for getting hurt
        Animator.Play("Hurt");

        // kill enememy if it died
        if (Actor.IsDead)
            Die();
    }
    protected virtual void Die() 
    {
        // play dead animation
        Animator.Play("Dead");
        // send death to manager to check for combat over
        manager.CheckCombatOver();

        // deactivate colliders
        foreach (var colider in Colliders)
            colider.gameObject.SetActive(false);
    }
    #endregion


}
