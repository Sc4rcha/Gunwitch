using System.Collections;
using GameInfo;
using UnityEngine;

public class CombatAgent : MonoBehaviour
{
    public Actor Actor { get; private set; }

    [Header ("Enemy Info")]
    public SOCombatEnemy EnemyStatsReference;
    public int Priority;

    [Header("Scene References")]
    public Transform Pivot;
    public Transform DamageMessagePosition;
    public Animator Animator;
    public BulletHitEffect BulletHitEffect;
    public SpriteRenderer Renderer;
    public SpriteRenderer[] RenderersExtra;
    public CombatAgentCollider[] Colliders;


    protected ManagerCombat manager;

    public bool IsHurt { get; protected set; }
    public bool IsDoingStuff { get; protected set; }

    private bool isPlayerTurn;
    private const float turnCooldown = 0.5f;

    protected float hurtAnimationTime = 0.5f;
    protected float deathAnimationTime = 0.75f;
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

        // setup main renderer
        Renderer.GetComponent<CombatAgentShake>().enabled = false;
        Renderer.color = Color.white;
        Renderer.sortingOrder = Priority * 10;
        // set extra sprite renderers order
        for (int i = 0; i < RenderersExtra.Length; i++)
            RenderersExtra[i].sortingOrder = Priority * 10 + i + 1;
        BulletHitEffect.SetMaskRange(Renderer.sortingOrder);

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
        manager.ScreenAttack.OnAnimationFinish -= TurnFinish;

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
        manager.ScreenAttack.Attack(EnemyStatsReference.AttackSprite, EnemyStatsReference.Attack);
        manager.ScreenAttack.OnAnimationFinish += TurnFinish;

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
            if (hitMessage.IsAvailable)
            {
                hitMessage.transform.position = DamageMessagePosition.position;
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

        // show bullet hit effect
        BulletHitEffect.Damage(Actor.IsDead);
    }
    protected virtual void Die() 
    {
        // delay death deactivate enemy
        StartCoroutine(DeathAnimation());
    }
    public void ForceKill() 
    {
        Actor.HealthChange(int.MinValue);
        gameObject.SetActive(false);
    }
    #endregion

    protected virtual void Cleanup() 
    {
        // add loot to win screen
        foreach (var loot in EnemyStatsReference.Loot)
            manager.ScreenWin.AddLootItem(loot);

        // deactivate colliders
        foreach (var colider in Colliders)
            colider.gameObject.SetActive(false);

        // send death to manager to check for combat over
        manager.CheckCombatOver();

        gameObject.SetActive(false);
    }

    public virtual void BulletHit(Vector2 mousePosition) 
    {
        BulletHitEffect.HitPlace(mousePosition);
    }

    protected virtual IEnumerator HurtAnimation(bool isCrit) 
    {
        IsDoingStuff = true;
        IsHurt = true;

        // start shake
        if (isCrit)
            Renderer.GetComponent<CombatAgentShake>().ShakeStart(2);
        else
            Renderer.GetComponent<CombatAgentShake>().ShakeStart(1);

        manager.Effects.HitStopStart();

        // color lerp
        hurtDeathAnimationTimeCurrent = 0;
        while (hurtDeathAnimationTimeCurrent < hurtAnimationTime)
        {
            Renderer.color = Color.Lerp(Color.softRed, Color.white, hurtDeathAnimationTimeCurrent / hurtAnimationTime);
            hurtDeathAnimationTimeCurrent += Time.deltaTime;
            yield return null;
        }

        IsDoingStuff = false;
        IsHurt = false;
    }
    protected virtual IEnumerator DeathAnimation()
    {
        IsDoingStuff = true;
        IsHurt = true;

        // start shake
        Renderer.GetComponent<CombatAgentShake>().ShakeStart(3);

        manager.Effects.HitStopStart();

        // color lerp
        hurtDeathAnimationTimeCurrent = 0;
        while (hurtDeathAnimationTimeCurrent < deathAnimationTime)
        {
            Renderer.color = Color.Lerp(Color.gray5, Color.white, hurtDeathAnimationTimeCurrent / deathAnimationTime);
            hurtDeathAnimationTimeCurrent += Time.deltaTime;
            yield return null;
        }

        IsDoingStuff = false;
        IsHurt = false;

        Cleanup();
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }
    protected virtual void DrawGizmos() 
    {
    }
#endif
}
