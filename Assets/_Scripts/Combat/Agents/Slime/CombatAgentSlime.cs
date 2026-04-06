using UnityEngine;

public class CombatAgentSlime : CombatAgent
{
    [Header("SLIME")]
    public CombatSlimeMovement Movement;
    [Space]
    [Range(0, 100)]
    public int ProbabilityJump;
    [Range(0, 100)]
    public int ProbabilityDirectionChange;
    [Range(0, 100)]
    public int ProbabilityPause;
    public float TimeToRandomize;

    // random movements
    private float timeToRandomizeCurrent;
    private int randomResult;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        Movement.Setup(this, manager.ArenaBounds);

        // start with randomizing movement
        timeToRandomizeCurrent = TimeToRandomize;
    }

    #region Player Turn
    public override void PlayerTurnFinish()
    {
        base.PlayerTurnFinish();

        Movement.Stop();
    }
    protected override void PlayerTurnBehaviour()
    {
        base.PlayerTurnBehaviour();

        // tick randomizer timer
        timeToRandomizeCurrent += Time.deltaTime;
        if (TimeToRandomize < timeToRandomizeCurrent)
        {
            // reset timer
            timeToRandomizeCurrent = 0;

            // get rn
            randomResult = Random.Range(0, 100);

            // do action depending on rn result
            if (randomResult < ProbabilityJump)
                Movement.Jump();
            else if (randomResult < ProbabilityDirectionChange)
                Movement.ToggleDirection();
            else if (randomResult < ProbabilityPause)
                Movement.ToggleMoving();
        }
    }
    #endregion

    #region Stat Change
    public override void Damage(int value)
    {
        base.Damage(value);

        // if hit while not moving and also didn't die from hit multiply
        if (!Movement.isMoving && !Actor.IsDead)
        {
            manager.Encounter.SpawnEnemy(this, transform.position, out CombatAgent addedEnemy);
            (addedEnemy as CombatAgentSlime).Movement.Spawn();
        }
    }
    protected override void Die()
    {
        base.Die();

        Movement.Stop();
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // fix movement probabilities
        ProbabilityDirectionChange = Mathf.Clamp(ProbabilityDirectionChange, ProbabilityJump, 100);
        ProbabilityPause = Mathf.Clamp(ProbabilityPause, ProbabilityDirectionChange, 100);
    }
#endif
}
