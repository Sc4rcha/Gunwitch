using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.Timeline;

public class CombatAgentBanditItem : CombatAgent
{
    [Header ("Bandit Item")]
    public float JumpHeight;
    public float TimeToJumpApex;
    public float TimeToChange;
    public float TimeToChangeVariance;
    public float RotateSpeed;
    [Space]
    public Transform handLeft;
    public Transform handRight;

    // move variables
    private float timeToChangeCurrent;
    private float moveTimeCurrent;
    private float gravity;
    private float jumpVelocity;

    private float startingX;
    private Vector3 enemyVelocity;
    private Vector3 enemyPosition;

    private enum MoveType {LeftToRight, RightToLeft}
    private MoveType moveType;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        // calculate gravity and jump speed
        gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;
    }

    #region Player turn
    public override void PlayerTurnStart()
    {
        base.PlayerTurnStart();

        // activate colliders
        foreach (var colider in Colliders)
            colider.gameObject.SetActive(true);

        // reset item
        gameObject.SetActive(true);
        Actor.Startcombat();

        // set random first position
        if (Random.Range(0, 2) > 1)
            transform.position = handLeft.position;
        else
            transform.position = handRight.position;
    }
    public override void PlayerTurnFinish()
    {
        base.PlayerTurnFinish();

        // move to left hand when player turn finished
        transform.position = handLeft.transform.position;
    }
    protected override void PlayerTurnUpdate()
    {
        base.PlayerTurnUpdate();

        enemyVelocity.y += gravity * Time.deltaTime;
        enemyPosition += enemyVelocity * Time.deltaTime;

        switch (moveType)
        {
            case MoveType.LeftToRight:
                // Move left to right
                moveTimeCurrent += Time.deltaTime;
                enemyPosition.x = Mathf.Lerp(startingX, handRight.position.x, moveTimeCurrent / (TimeToJumpApex * 2));
                enemyPosition.y = Mathf.Clamp(enemyPosition.y, handRight.position.y, Mathf.Infinity);
                if (enemyPosition.y > handRight.position.y)
                    transform.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);
                else
                    Animator.Play("Hidden");
                 break;
            case MoveType.RightToLeft:
                // Move right to left
                moveTimeCurrent += Time.deltaTime;
                enemyPosition.x = Mathf.Lerp(startingX, handLeft.position.x, moveTimeCurrent / (TimeToJumpApex * 2));
                enemyPosition.y = Mathf.Clamp(enemyPosition.y, handLeft.position.y, Mathf.Infinity);
                if (enemyPosition.y > handLeft.position.y)
                    transform.Rotate(0f, 0f, -RotateSpeed * Time.deltaTime);
                else
                    Animator.Play("Hidden");
                break;
        }

        // change state
        timeToChangeCurrent += Time.deltaTime;
        if (TimeToChange < timeToChangeCurrent)
            GetRandomMovement();

        transform.position = enemyPosition;
    }
    private void GetRandomMovement()
    {
        // reset move variables
        timeToChangeCurrent = Random.Range(0, TimeToChangeVariance);
        moveTimeCurrent = 0;
        enemyVelocity.y = jumpVelocity;
        startingX = transform.position.x;

        Animator.Play("Idle");

        // set random move type
        moveType = (MoveType)Random.Range(0, 2);
    }
    #endregion


}
