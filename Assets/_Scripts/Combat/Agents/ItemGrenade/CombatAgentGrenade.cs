using UnityEngine;

public class CombatAgentGrenade : CombatAgent
{
    [Header ("Grenade")]
    public GameObject ExplosionEffect;
    [Space]
    public float JumpHeight;
    public float TimeToJumpApex;
    [Space]
    public SOInventoryItemBullet Explosion;

    private bool wasShot;
    private float gravity;
    private float jumpVelocity;
    private Vector3 velocity;

    private Bounds bounds;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        bounds = manager.ArenaBounds;

        // calculate gravity and jump speed
        gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;

        // set velocity to "jump" at the Setup
        velocity = Vector2.up * jumpVelocity;

        // set as not shot
        wasShot = false;
    }

    protected override void PlayerTurnUpdate()
    {
        base.PlayerTurnUpdate();

        if (IsHurt)
            return;

        // update velocity
        velocity.y += gravity * Time.deltaTime;

        // move grenade
        transform.position += velocity * Time.deltaTime;

        if (transform.position.y < bounds.min.y)
            Die();
    }


    // grenade shot, do extra damage!
    public override void Damage(int damage, bool isCrit)
    {
        base.Damage(damage, isCrit);

        wasShot = true;
    }


    // explode on grenade cleanup
    protected override void Cleanup()
    {
        base.Cleanup();

        Explode();
    }

    private void Explode() 
    {
        // show effect
        Instantiate(ExplosionEffect, References.Pivot.position, Quaternion.identity, manager.Encounter.transform);

        // deal damage
        foreach (var agent in manager.Encounter.Enemies)
        {
            if (agent != this)
                agent.Damage(Explosion.GetBullet(), wasShot);
        }
    }
}
