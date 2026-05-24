using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatSlimeMovement : MonoBehaviour
{
    public Transform Body;
    [Space]
    public float MoveSpeed;
    public float JumpHeightMax;
    public float JumpHeightMin;
    public float TimeToJumpApex;
    public float HorizontalMovementSmoothTime;

    // jump variables
    private float gravity;
    private float jumpVelocityMax;
    private float jumpVelocityMin;

    public bool IsJumping => enemyPosition.y > bounds.min.y;

    // movement variables
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
        gravity = -(2 * JumpHeightMax) / Mathf.Pow(TimeToJumpApex, 2);
        jumpVelocityMax = Mathf.Abs(gravity) * TimeToJumpApex;
        jumpVelocityMin = jumpVelocityMax / JumpHeightMax * JumpHeightMin;
    }

    #region MOVEMENT ACTIONS
    public void Jump()
    {
        // stop jump if already jumping
        if (IsJumping)
            return;

        ForceJumpState();

        if (Mathf.Abs(Body.position.x - bounds.center.x) > bounds.size.x / 4)
            enemyVelocity.x = MoveSpeed * -Mathf.Sign(Body.position.x - bounds.center.x);
        else 
        {
            enemyVelocity.x = MoveSpeed;
            if (Random.Range(0, 2) == 1)
                enemyVelocity.x = -enemyVelocity.x;
        }

        enemyVelocity.y = Random.Range(jumpVelocityMin, jumpVelocityMax);
        manager.References.Animator.Play("Jump");
    }
    public void Spawn() 
    {
        Jump();
    }
    public void ForceJumpState() 
    {
        enemyPosition.y += 0.1f;
    }
    #endregion


    public void Update()
    {
        if (manager.IsHurt)
            return;

        // apply horizontal smooth damp stop
        if (!IsJumping)
            enemyVelocity.x = Mathf.SmoothDamp(enemyVelocity.x, 0, ref horizontalDampVelocity, HorizontalMovementSmoothTime);

        CalculateMovement();

        // set animator varialbes
        manager.References.Animator.SetBool("isJumping", IsJumping);
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
}
