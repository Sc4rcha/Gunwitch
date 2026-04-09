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
        // clear attack screen finish event
        manager.ScreenAttack.OnAttackAnimationEnd -= TurnFinish;

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
    }
    protected virtual void Act()
    {
        // visual feedback for acting
        manager.ScreenAttack.Attack(EnemyStatsReference.AttackSprite);
        manager.ScreenAttack.OnAttackAnimationEnd += TurnFinish;

        // Do ability
        manager.EnemyAttack(EnemyStatsReference.Attack);
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
        foreach (var hitMessage in manager.HitMessages)
        {
            if (!hitMessage.gameObject.activeInHierarchy)
            {
                hitMessage.transform.position = manager.Player.Gun.Crosshair.Crosshair.transform.position + Vector3.up;
                hitMessage.ShowNumber(value);
                break;
            }
        }

        Actor.HealthChange(-value);

        // visual feedback for getting hurt
        Animator.Play("Hurt");

        // kill enememy if it died
        if (Actor.IsDead)
            Die();
    }
    protected virtual void Die() 
    {
        // add loot to win screen
        foreach (var loot in EnemyStatsReference.Loot)
            manager.ScreenWin.AddLootItem(loot);

        // send death to manager to check for combat over
        manager.CheckCombatOver();

        // deactivate colliders
        foreach (var colider in Colliders)
            colider.gameObject.SetActive(false);


        // play dead animation
        Animator.Play("Dead");

        // delay death deactivate enemy
        StartCoroutine(DeathAnimationDelay());
    }
    #endregion


    IEnumerator DeathAnimationDelay()
    {
        while (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            yield return null;

        while (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        gameObject.SetActive(false);
    }

}
