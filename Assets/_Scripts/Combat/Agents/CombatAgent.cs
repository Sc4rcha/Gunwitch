using GameInfo;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatAgent : MonoBehaviour
{
    public ActorEnemy Actor { get; private set; }

    [Header ("Enemy Info")]
    public SOCombatEnemy EnemyStatsReference;
    public int Priority;

    protected ManagerCombat manager;
    protected CombatAgentShake shake;

    public CombatAgentReferences References { get; protected set; }

    public bool IsHurt { get; protected set; }
    public bool IsDoingStuff { get; protected set; }

    private bool isPlayerTurn;
    private const float turnCooldown = 0.5f;

    protected int momentum;

    protected float hurtAnimationTime = 0.5f;
    private float hurtAnimationTimeCurrent;
    private WaitForSeconds deathEffectTime;
    private bool isActorShot;

    public virtual void Setup(ManagerCombat manager)
    {
        // set manager reference
        this.manager = manager;

        // get references
        References = GetComponent<CombatAgentReferences>();

        // Setup colliders
        foreach (var collider in References.Colliders)
            collider.Setup(this);

        // setup agent stats
        Actor = EnemyStatsReference.GetCombatActor();
        Actor.Startcombat();
        momentum = 0;

        // setup shaker
        shake = References.Renderer.GetComponent<CombatAgentShake>();
        shake.enabled = false;

        SetOrderInLayer(Priority * 100);

        // setup damage varialbes
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


    public void SetOrderInLayer(int enemyOrderInLayer) 
    {
        // setup renderer
        References.Renderer.color = Color.white;
        References.Renderer.sortingOrder = enemyOrderInLayer;
        // set extra sprite renderers order
        for (int i = 0; i < References.RenderersExtra.Length; i++)
            References.RenderersExtra[i].sortingOrder = Priority + i + 1;

        // set effects
        References.BulletHitEffect.Setup(References.Renderer.sortingOrder);

        // set renderer sprite mask order in layer
        References.Renderer.GetComponent<SpriteMask>().frontSortingOrder = References.BulletHitEffect.HitHoleOrderInLayer + 1;
        References.Renderer.GetComponent<SpriteMask>().backSortingOrder = References.BulletHitEffect.HitHoleOrderInLayer - 1;
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

        // add momentum
        momentum += Actor.Speed;

        // start turn behaviour coroutine
        StartCoroutine(TurnBehaviour());
    }
    public virtual void TurnFinish()
    {
        // clear attack screen finish event
        manager.ScreenAttack.OnAnimationFinish -= TurnFinish;

        if (CombatMethods.EnemyExtraTurn(ref momentum, Actor, manager.Configuration))
            // act again if extra turn, start turn behaviour coroutine
            StartCoroutine(TurnBehaviour());
        else
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
    public virtual void Damage(int damage, bool isCrit)
    {
        foreach (var hitMessage in manager.HitMessages)
        {
            if (hitMessage.IsAvailable)
            {
                hitMessage.transform.position = References.MarkerDamage.position;

                if (!isCrit)
                    hitMessage.ShowNumber(damage.ToString(), 1, CombatHitMessage.MessageType.Damage);
                else
                    hitMessage.ShowNumber(damage.ToString(), 2, CombatHitMessage.MessageType.Damage);

                break;
            }
        }

        Actor.HealthChange(-damage);

        // kill enememy if it died otherwise play hurt animation
        if (Actor.IsDead)
            Die();
        else
            StartCoroutine(HurtAnimation(isCrit));


        // refresh UI Status Effects
        References.AgentUI.RefreshStatusEffects(Actor, manager.Configuration);
    }
    public virtual void Damage(Bullet bullet, bool isCrit)
    {
        int damage = (int)CombatMethods.DamageBullet(manager.Player.PlayerReference.Actor, Actor, bullet, manager.Player.Gun.CritMultiplier, isCrit, manager.Configuration);

        Damage(damage, isCrit);
    }
    public virtual void Damage(Bullet bullet, bool isCrit, ref int damage)
    {
        damage = (int)CombatMethods.DamageBullet(manager.Player.PlayerReference.Actor, Actor, bullet, manager.Player.Gun.CritMultiplier, isCrit, manager.Configuration);

        Damage(damage, isCrit);
    }
    public virtual void Damage(ActorEnemy enemy ,SOEnemySkill skill)
    {
        int damage = (int)CombatMethods.DamageEnemyToEnemy(enemy, Actor, skill, manager.Configuration);

        Damage(damage, false);
    }
    public virtual void BulletHit(Vector2 mousePosition)
    {
        isActorShot = true;
        References.BulletHitEffect.HitPlace(mousePosition);
    }

    public void AddStatusEffect(StatusEffect statusEffect) 
    {
        Actor.StatusEffects.Add(statusEffect);
        References.AgentUI.RefreshStatusEffects(Actor, manager.Configuration);
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
        foreach (var colider in References.Colliders)
            colider.gameObject.SetActive(false);

        // send death to manager to check for combat over
        manager.CheckCombatOver();

        gameObject.SetActive(false);
    }

    protected virtual IEnumerator HurtAnimation(bool isCrit)
    {
        // show bullet hit effect
        if (isActorShot)
            References.BulletHitEffect.Damage();

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
            References.Renderer.color = Color.Lerp(Color.softRed, Color.white, hurtAnimationTimeCurrent / hurtAnimationTime);
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
            References.BulletHitEffect.Damage();

        IsDoingStuff = true;
        IsHurt = true;

        // start shake
        manager.Effects.HitStopStart();

        if (isActorShot)
        {
            // color lerp
            hurtAnimationTimeCurrent = 0;
            while (hurtAnimationTimeCurrent < hurtAnimationTime)
            {
                References.Renderer.color = Color.Lerp(Color.gray5, Color.white, hurtAnimationTimeCurrent / hurtAnimationTime);
                hurtAnimationTimeCurrent += Time.deltaTime;
                yield return null;
            }

            // death effect
            References.BulletHitEffect.Die();

            manager.Effects.HitStopStart();
            shake.ShakeStart(3);

            yield return deathEffectTime;
        }
        else
        {
            // death effect
            manager.Effects.HitStopStart();
            shake.ShakeStart(2);

            // color lerp
            hurtAnimationTimeCurrent = 0;
            while (hurtAnimationTimeCurrent < hurtAnimationTime)
            {
                References.Renderer.color = Color.Lerp(Color.gray5, Color.white, hurtAnimationTimeCurrent / hurtAnimationTime);
                hurtAnimationTimeCurrent += Time.deltaTime;
                yield return null;
            }
        }

        isActorShot = false;
        IsDoingStuff = false;
        IsHurt = false;

        Cleanup();
    }
    #endregion

    public void SelectActing (bool isSelect)
    {
        References.Renderer.material.SetFloat("_OutlineSize", 20);

        if (isSelect)
        {
            References.Renderer.material.SetInt("_OutlineOn", 1);
            References.Animator.transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            References.Renderer.material.SetInt("_OutlineOn", 0);
            References.Animator.transform.localScale = Vector3.one;
        }
    }
    public void SelectTarget(bool isSelect)
    {
        References.Renderer.material.SetFloat("_OutlineSize", 10);

        if (isSelect)
        {
            References.Renderer.material.SetInt("_OutlineOn", 1);
            References.Animator.transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            References.Renderer.material.SetInt("_OutlineOn", 0);
            References.Animator.transform.localScale = Vector3.one;
        }
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
