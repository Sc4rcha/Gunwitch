using UnityEngine;

public class CombatAgentSlime : CombatAgent
{
    [Header("SLIME")]
    public SpriteRenderer RendererCore;
    public CombatSlimeMovement Movement;
    [Space]
    [Range(0, 100)]
    public int ProbabilityJump;
    public float TimeToRandomize;
    public float TimeToRandomizeVariance;

    // random movements
    protected float timeToRandomizeCurrent;
    protected int randomResult;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        RendererCore.sortingOrder = Renderer.sortingOrder + 1;

        Movement.Setup(this, manager.ArenaBounds);

        // start with randomizing movement
        timeToRandomizeCurrent = TimeToRandomize;
    }

    #region Player Turn
    protected override void PlayerTurnUpdate()
    {
        base.PlayerTurnUpdate();

        // tick randomizer timer
        timeToRandomizeCurrent += Time.deltaTime;
        if (TimeToRandomize < timeToRandomizeCurrent)
        {
            // reset timer with variance
            timeToRandomizeCurrent = Random.Range (0, TimeToRandomizeVariance);

            // get rn
            randomResult = Random.Range(0, 100);

            // do action depending on rn result
            if (randomResult < ProbabilityJump)
                Movement.Jump();
        }
    }
    #endregion


    #region Stat Change
    public override void Damage(int value, bool isCrit)
    {
        base.Damage(value, isCrit);

        // if hit while not moving and also didn't die from hit multiply
        if (!Movement.IsJumping)
        {
            manager.Encounter.SpawnEnemy(this, transform.position, out CombatAgent addedEnemy);
            (addedEnemy as CombatAgentSlime).Movement.Spawn();
        }
    }
    #endregion
}
