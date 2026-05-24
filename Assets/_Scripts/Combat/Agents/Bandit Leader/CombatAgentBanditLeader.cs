using GameInfo;
using UnityEditor;
using UnityEngine;

public class CombatAgentBanditLeader : CombatAgent
{
    [Header("Bandit Leader")]
    public CombatAgentBanditItem[] Items;
    public CombatAgentBanditLeaderSword Sword;
    [Space]
    public Transform BanditSpawnPosition;
    public CombatAgentBandit BanditSpawnReference;
    public CombatAgent MeatShield;
    public SOCombatEnemy MeatShieldReference;
    public Bounds MeatShieldBounds;

    private bool previousTurnNoBandit;
    private int itemIndex = 0;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        // setup items
        foreach (var item in Items)
            item.Setup(manager);

        // setup meat shield and deactivate it
        MeatShield.Setup(manager);
        MeatShield.gameObject.SetActive(false);
        manager.Encounter.AddSubEnemyToEncounter(MeatShield);

        // set spawn bandit bool to NOT SPAWN
        previousTurnNoBandit = false;
    }


    #region Player Turn
    public override void PlayerTurnStart()
    {
        base.PlayerTurnStart();

        // item player turn start (start Movement)
        foreach (var item in Items)
            item.PlayerTurnStart();

        // if previous turn had no bandit spawn it.
        if (previousTurnNoBandit)
        {
            if (!manager.Encounter.CheckForEnemy(MeatShieldReference.Id))
                manager.Encounter.SpawnEnemy(BanditSpawnReference, BanditSpawnPosition.position);
        }
        
        // check if there is a bandit on the encounter
        previousTurnNoBandit = manager.Encounter.CheckForEnemy(MeatShieldReference.Id) == null;
    }
    public override void PlayerTurnFinish()
    {
        base.PlayerTurnFinish();

        // item player turn finish (finish Movement)
        foreach (var item in Items)
            item.PlayerTurnFinish();

        // hide meatshield
        MeatShield.gameObject.SetActive(false);
        References.Animator.Play("Idle");

    }
    #endregion


    public override void TurnStart()
    {
        if (!Items[itemIndex].Actor.IsDead)
        {
            itemIndex++;
            base.TurnStart();
        }
        else
        {
            itemIndex++;
            TurnFinish();
        }
    }
    public override void TurnFinish()
    {
        // check next item
        if (itemIndex < Items.Length)
        {
            manager.ScreenAttack.OnAnimationFinish -= TurnFinish;
            TurnStart();
            return;
        }

        base.TurnFinish();

        itemIndex = 0;

        // try and get new sword if player broke it
        if (Sword.Actor.IsDead)
            Sword.Revive();
    }
    protected override SOEnemyAction SelectAction()
    {
        return Items[itemIndex].SelectedAction;
    }


    #region Stat change
    public override void Damage(Bullet bullet, bool isCrit, ref int damage)
    {
        if (MeatShield.isActiveAndEnabled)
            MeatShield.Damage(bullet, false, ref damage);
        else
            base.Damage(bullet, isCrit, ref damage);
    }
    public override void BulletHit(Vector2 mousePosition)
    {
        // Move meatshield
        if (MeatShield.isActiveAndEnabled)
        {
            Vector3 meatshieldPosition = MeatShieldBounds.ClosestPoint(mousePosition);
            MeatShield.transform.position = new Vector3(meatshieldPosition.x, meatshieldPosition.y, MeatShield.transform.position.z);
            return;
        }

        // Get new meatshield
        if (manager.Encounter.CheckForEnemy(MeatShieldReference.Id) is CombatAgent meatShield)
        {
            References.Animator.Play("Guard");

            meatShield.ForceKill();
            MeatShield.Setup(manager);
            MeatShield.gameObject.SetActive(true);
            MeatShield.BulletHit(mousePosition);

            MeatShield.transform.position = new Vector3(mousePosition.x, mousePosition.y, MeatShield.transform.position.z);

            foreach (var item in Items)
                item.gameObject.SetActive(false);

            return;
        }

        // otherwise normal bullet hit
        base.BulletHit(mousePosition);
    }

    protected override void Die()
    {
        base.Die();

        // kill sword
        Sword.ForceKill();

        // kill meatshield
        MeatShield.ForceKill();
    }
    #endregion


    protected override void DrawGizmos()
    {
        base.DrawGizmos();

        Handles.DrawWireCube(MeatShieldBounds.center, MeatShieldBounds.size);
    }
}
