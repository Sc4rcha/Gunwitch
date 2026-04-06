using UnityEditor;
using UnityEngine;

public class CombatSlimeMovement : MonoBehaviour
{
    public Transform Body;
    [Space]
    public float MoveSpeed;
    public float JumpHeight;
    public float TimeToJumpApex;
    public float HorizontalMovementSmoothTime;

    // jump variables
    private float gravity;
    private float jumpVelocity;

    // move states
    public bool isMoving { get; private set; }
    private bool isMovingRight;
    private bool isJumping => enemyPosition.y > bounds.min.y;

    // movement variables
    private float targetVelocityX;
    private float horizontalDampVelocity;
    private Vector3 enemyVelocity;
    private Vector3 enemyPosition;
    private Bounds bounds;

    private CombatAgentSlime manager;

    public void Setup(CombatAgentSlime manager, Bounds bounds) 
    {
        this.manager = manager;
        this.bounds = bounds;

        // setup position
        enemyPosition = Body.position;

        // calculate gravity and jump speed
        gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;
    }

    #region MOVEMENT ACTIONS
    public void Stop() 
    {
        targetVelocityX = 0;
    }
    public void Jump()
    {
        enemyVelocity.y = jumpVelocity;
        manager.Animator.Play("Jump");
    }
    public void ToggleMoving() 
    {
        isMoving = !isMoving;

        if (isMoving)
        {
            if (isMovingRight)
                targetVelocityX = MoveSpeed;
            else
                targetVelocityX = -MoveSpeed;
        }
        else
            targetVelocityX = 0;

    }
    public void ToggleDirection()
    {
        isMovingRight = !isMovingRight;
        targetVelocityX *= -1;
    }
    public void Spawn() 
    {
        enemyVelocity.x = Random.Range(-1, 1) * MoveSpeed;
        targetVelocityX = enemyVelocity.x;
        Jump();
    }
    #endregion


    private void Update()
    {
        UpdateMove();
        CalculateMovement();

        // set animator varialbes
        manager.Animator.SetFloat("velocityX", enemyVelocity.x);
        manager.Animator.SetBool("isJumping", isJumping);
    }


    #region move updates
    private void UpdateMove()
    {
        // apply horizontal smooth damp
        enemyVelocity.x = Mathf.SmoothDamp(enemyVelocity.x, targetVelocityX, ref horizontalDampVelocity, HorizontalMovementSmoothTime);

        // if is not jumping change direction when out of bounds
        if (!isJumping)
        {
            if (bounds.min.x > Body.position.x && !isMovingRight)
                ToggleDirection();
            else if (bounds.max.x < Body.position.x && isMovingRight)
                ToggleDirection();
        }
    }
    private void CalculateMovement() 
    {
        // apply gravity
        enemyVelocity.y += gravity * Time.deltaTime;

        // move enemy
        enemyPosition += enemyVelocity * Time.deltaTime;

        // snap to ground
        enemyPosition.y = Mathf.Clamp(enemyPosition.y, bounds.min.y, Mathf.Infinity);

        // apply position
        Body.position = enemyPosition;
    }
    #endregion

}
