using GameInfo;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;

public class CombatAgent : MonoBehaviour
{
    public ActorEnemy Actor { get; private set; }

    [Header ("Enemy Info")]
    public SOCombatEnemy EnemyStatsReference;
    public int Priority;

    [Header("Scene References")]
    public Transform Pivot;
    public Transform MarkerDamage;
    public Transform MarkerSelect;
    [Space]
    public Animator Animator;
    public BulletHitEffect BulletHitEffect;
    public SpriteRenderer Renderer;
    public SpriteRenderer[] RenderersExtra;
    [Space]
    public CombatAgentCollider[] Colliders;

    protected ManagerCombat manager;
    protected CombatAgentShake shake;

    public bool IsHurt { get; protected set; }
    public bool IsDoingStuff { get; protected set; }

    private bool isPlayerTurn;
    private const float turnCooldown = 0.5f;

    protected float hurtAnimationTime = 0.5f;
    private float hurtAnimationTimeCurrent;
    private WaitForSeconds deathEffectTime;
    private bool isActorShot;

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

        // setup shaker
        shake = Renderer.GetComponent<CombatAgentShake>();
        shake.enabled = false;

        // setup renderer
        Renderer.color = Color.white;
        Renderer.sortingOrder = Priority * 10;
        // set extra sprite renderers order
        for (int i = 0; i < RenderersExtra.Length; i++)
            RenderersExtra[i].sortingOrder = Priority * 10 + i + 1;

        // setup damage varialbes
        BulletHitEffect.Setup(Renderer.sortingOrder);
        deathEffectTime = new WaitForSeconds(0.75f);
        IsHurt = false;

        // set turn variables
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

        // send turn finish to manager for next enemy to act
        manager.EnemyTurnFinish();
    }
    private IEnumerator TurnBehaviour()
    {
        // initial wait before taking action
        yield return new WaitForSeconds(turnCooldown);

        // enemy takes action
        Act(SelectAction());
    }
    protected virtual SOEnemyAction SelectAction() 
    {
        List<SOEnemyAction> validActions = EnemyStatsReference.Actions.Where(a => a.CheckConditions(manager.Encounter, Actor)).ToList();

        if (validActions.Count == 0)
            return null;

        return CombatMethods.SelectByRating(validActions);
    }
    protected virtual void Act(SOEnemyAction action)
    {
        // if no actions found skip turn
        if (action == null)
            TurnFinish();

        // visual feedback for acting
        manager.ScreenAttack.Attack(this, action.Skill);
        manager.ScreenAttack.OnAnimationFinish += TurnFinish;
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


    #region HEALTH / DEATH
    public virtual void Damage(int value, bool isCrit)
    {
        foreach (var hitMessage in manager.HitMessages)
        {
            if (hitMessage.IsAvailable)
            {
                hitMessage.transform.position = MarkerDamage.position;

                if (!isCrit)
                    hitMessage.ShowNumber(value.ToString(),1);
                else
                    hitMessage.ShowNumber(value.ToString(), 2);

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
    public virtual void BulletHit(Vector2 mousePosition)
    {
        isActorShot = true;
        BulletHitEffect.HitPlace(mousePosition);
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

    protected virtual IEnumerator HurtAnimation(bool isCrit)
    {
        // show bullet hit effect
        if (isActorShot)
            BulletHitEffect.Damage();

        IsDoingStuff = true;
        IsHurt = true;

        // start shake
        if (isCrit)
            shake.ShakeStart(2);
        else
            shake.ShakeStart(1);

        manager.Effects.HitStopStart();

        // color lerp
        hurtAnimationTimeCurrent = 0;
        while (hurtAnimationTimeCurrent < hurtAnimationTime)
        {
            Renderer.color = Color.Lerp(Color.softRed, Color.white, hurtAnimationTimeCurrent / hurtAnimationTime);
            hurtAnimationTimeCurrent += Time.deltaTime;
            yield return null;
        }

        isActorShot = false;
        IsDoingStuff = false;
        IsHurt = false;
    }
    protected virtual IEnumerator DeathAnimation()
    {
        // show bullet hit effect
        if (isActorShot)
            BulletHitEffect.Damage();

        IsDoingStuff = true;
        IsHurt = true;

        // start shake
        manager.Effects.HitStopStart();

        // color lerp
        hurtAnimationTimeCurrent = 0;
        while (hurtAnimationTimeCurrent < hurtAnimationTime)
        {
            Renderer.color = Color.Lerp(Color.gray5, Color.white, hurtAnimationTimeCurrent / hurtAnimationTime);
            hurtAnimationTimeCurrent += Time.deltaTime;
            yield return null;
        }

        // show bullet hole effect
        if (isActorShot)
        {
            BulletHitEffect.Die();

            manager.Effects.HitStopStart();
            shake.ShakeStart(3);

            yield return deathEffectTime;
        }

        isActorShot = false;
        IsDoingStuff = false;
        IsHurt = false;

        Cleanup();
    }
    #endregion


    public void Select (bool isSelect)
    {
        if (isSelect)
            Animator.transform.localScale = Vector3.one * 1.1f;
        else
            Animator.transform.localScale = Vector3.one;
    }
    public void TakeAction() 
    {
        shake.ShakeSelect();
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
