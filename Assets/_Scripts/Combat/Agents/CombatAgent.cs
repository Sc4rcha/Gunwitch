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

    public bool IsHurt { get; private set; }

    private bool isPlayerTurn;
    private const float turnCooldown = 0.5f;

    private float hurtDeathAnimationTime = 0.5f;
    private float hurtDeathAnimationTimeCurrent;

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

        // set renderer variables
        Renderer.sortingOrder = Priority * 10;
        Renderer.color = Color.white;
        Renderer.GetComponent<CombatAgentShake>().enabled = false;


        IsHurt = false;
        isPlayerTurn = false;
    }


    private void Update()
    {
        if (isPlayerTurn && !Actor.IsDead)
            PlayerTurnUpdate();
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
        manager.ScreenAttack.OnAnimationEnd -= TurnFinish;

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
        manager.ScreenAttack.OnAnimationEnd += TurnFinish;

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
    protected virtual void PlayerTurnUpdate() { }
    #endregion


    #region Stat change
    public virtual void Damage(int value, bool isCrit)
    {
        foreach (var hitMessage in manager.HitMessages)
        {
            if (!hitMessage.gameObject.activeInHierarchy)
            {
                hitMessage.transform.position = manager.Player.Gun.Visuals.Crosshair.transform.position + Vector3.up;
                hitMessage.ShowNumber(value);
                break;
            }
        }

        Actor.HealthChange(-value);

        // kill enememy if it died otherwise play hurt animation
        if (Actor.IsDead)
            Die();
        else
            StartCoroutine(HurtAnimation(isCrit));
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

        // delay death deactivate enemy
        StartCoroutine(DeathAnimation());
    }
    #endregion


    IEnumerator HurtAnimation(bool isCrit) 
    {
        IsHurt = true;

        // start shake
        if (isCrit)
            Renderer.GetComponent<CombatAgentShake>().ShakeStart(2);
        else
            Renderer.GetComponent<CombatAgentShake>().ShakeStart(1);

        // color lerp
        hurtDeathAnimationTimeCurrent = 0;
        while (hurtDeathAnimationTimeCurrent < hurtDeathAnimationTime)
        {
            Renderer.color = Color.Lerp(Color.softRed, Color.white, hurtDeathAnimationTimeCurrent / hurtDeathAnimationTime);
            hurtDeathAnimationTimeCurrent += Time.deltaTime;
            yield return null;
        }

        // Hide bullet stencil
        manager.Player.Gun.Visuals.HideStencil();

        IsHurt = false;
    }
    IEnumerator DeathAnimation()
    {
        IsHurt = true;

        // start shake
        Renderer.GetComponent<CombatAgentShake>().ShakeStart(3);

        // color lerp
        hurtDeathAnimationTimeCurrent = 0;
        while (hurtDeathAnimationTimeCurrent < hurtDeathAnimationTime)
        {
            Renderer.color = Color.Lerp(Color.gray5, Color.white, hurtDeathAnimationTimeCurrent / hurtDeathAnimationTime);
            hurtDeathAnimationTimeCurrent += Time.deltaTime;
            yield return null;
        }

        // Hide bullet stencil
        manager.Player.Gun.Visuals.HideStencil();

        IsHurt = false;

        gameObject.SetActive(false);
    }

}
