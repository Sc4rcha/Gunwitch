using System.Collections;
using GameInfo;
using GymCombat;
using UnityEngine;

public class CombatAgent : MonoBehaviour
{
    public CombatActor Stats { get; private set; }


    [Header ("Enemy Info")]
    public SOCombatEnemy EnemyStatsReference;

    [Header ("Scene References")]
    public SpriteRenderer Renderer;
    public CombatAgentCollider[] Colliders;

    protected Animator animator;
    protected ManagerCombat manager;

    private const float turnCooldown = 1;

    public virtual void Setup(ManagerCombat manager)
    {
        // set references
        this.manager = manager;
        animator = Renderer.GetComponent<Animator>();

        // Setup colliders
        foreach (var collider in Colliders)
            collider.Setup(this);

        // setup agent stats
        Stats = EnemyStatsReference.GetCombatActor();
        Stats.Startcombat();
    }

    public void TurnStart()
    {
        TurnBegin();
    }
    public void TurnFinish()
    {
        // send turn finish to manager for next enemy to act
        manager.EnemyTurnFinish();
    }


    public void Damage(int value)
    {
        Stats.HealthChange(-value);

        // visual feedback for getting hurt
        animator.Play("Hurt");

        // kill enememy if it died
        if (Stats.IsDead)
            Die();
    }
    private void Die() 
    {
        // play dead animation
        animator.Play("Dead");
        // send death to manager to check for combat over
        manager.CheckCombatOver();
    }

    #region TURN
    protected virtual void TurnBegin()
    {
        // start turn behaviour coroutine
        StartCoroutine(TurnBehaviour());
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
        TurnEnd();
    }
    protected virtual void TurnEnd()
    {
        // do turn end stuff

        // go to finish turn
        TurnFinish();
    }
    #endregion

    protected virtual void Act()
    {
        // visual feedback for acting
        animator.Play("Attack");

        // do 2 damage to player by default
        manager.Player.Damage(2);
    }
}
