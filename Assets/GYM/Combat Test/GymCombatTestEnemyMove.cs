using GymCombat;
using System.Collections;
using UnityEngine;

public class GymCombatTestEnemyMove : GymCombatTestEnemy
{
    public Vector2[] PatrolPoints;
    public float MoveTime;
    public float StopTime;

    private Vector2 initialPosition;
    private Vector2 previousStopPoint;
    private float moveTimeCurrent;
    private bool isStop;

    private int patrolIndex;

    public override void Setup(GymCombatTest manager)
    {
        base.Setup(manager);

        patrolIndex = 0;
        initialPosition = transform.position;
        transform.position = initialPosition + PatrolPoints[patrolIndex];
        StartCoroutine(Stop());
    }

    private void Update()
    {
        if (isStop || Enemy.IsDead)
            return;

        moveTimeCurrent += Time.deltaTime;
        transform.position = Vector2.Lerp(previousStopPoint, PatrolPoints[patrolIndex] + initialPosition, moveTimeCurrent / MoveTime);

        if (moveTimeCurrent > MoveTime)
            StartCoroutine(Stop());
    }

    private void PatrolNext()
    {
        patrolIndex = (patrolIndex + 1) % PatrolPoints.Length;
    }

    private IEnumerator Stop() 
    {
        isStop = true;
        previousStopPoint = PatrolPoints[patrolIndex] + initialPosition;

        yield return new WaitForSeconds(StopTime);

        PatrolNext();

        isStop = false;
        moveTimeCurrent = 0;
    }
}
