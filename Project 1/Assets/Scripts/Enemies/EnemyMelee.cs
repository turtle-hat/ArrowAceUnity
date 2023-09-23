using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enemy that waits until the player moves in front of it, then charges rightward across the screen
/// </summary>
public class EnemyMelee : Enemy
{
    [Tooltip("The difference in pixels between the enemy's and player's Y positions that will cause the enemy to charge")]
    public float chargeRange = 64f;
    [Tooltip("The amount of seconds it takes for the enemy to reappear onscreen after charging")]
    public float chargeReturnTime = 2f;
    private float chargeReturnTimer = 0f;
    [Tooltip("Position the enemy starts at")]
    public Vector2 startPosition = Vector2.zero;
    private float hurtEffectTime = 0.1f;
    private float hurtEffectTimer = 0f;

    // An overridable method called within Enemy.Start() for initializing stuff
    public override void SpawnBehavior()
    {
        movement.AccelerationTime = 0f;
        movement.DampingTime = 1f;
        movement.TopSpeed = 100f;
        movement.velocity = new Vector2(movement.TopSpeed, 0);
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            // Upon entering the screen, move until close enough to a formation line
            case EnemyState.Entering:
                if ((startPosition.x < formationLine && transform.position.x <= formationLine) ||
                    (startPosition.x > formationLine && transform.position.x >= formationLine))
                {
                    movement.Move(new Vector2(Mathf.Sign(formationLine - startPosition.x), 0));
                }
                else
                {
                    movement.Move(Vector2.zero);
                    phase = EnemyState.AttackWaiting;
                }
                break;
            // Wait until Player flies in front of enemy
            case EnemyState.AttackWaiting:
                movement.Move(Vector2.zero);

                if (player != null && Mathf.Abs(player.transform.position.y - transform.position.y) <= chargeRange)
                {
                    movement.TopSpeed = 400f;
                    movement.velocity = new Vector2(-(movement.TopSpeed / 2), 0);
                    movement.AccelerationTime = 1f;
                    movement.DampingTime = 0.5f;
                    phase = EnemyState.Attacking;
                }
                break;
            // Charge at Player until offscreen
            case EnemyState.Attacking:
                movement.Move(Vector2.right);

                foreach (PhysicsBox phys in collisionHandler.physicsBoxes)
                {
                    if (phys.GetDistanceOffscreen().x < -64)
                    {
                        movement.velocity = Vector2.zero;
                        chargeReturnTimer = chargeReturnTime;
                        phase = EnemyState.Recovering;
                    }
                }
                break;
            // Wait an amount of time, then return to original spot
            case EnemyState.Recovering:
                chargeReturnTimer -= Time.deltaTime;

                if (chargeReturnTimer <= 0)
                {
                    transform.position = startPosition;
                    movement.AccelerationTime = 0;
                    movement.DampingTime = 1f;
                    movement.TopSpeed = 100f;
                    phase = EnemyState.Entering;
                }

                break;
        }

        collisionHandler.ResetCollisions();
        collisionManager.CheckCollisionsOnHandler(collisionHandler);

        if (collisionHandler.collisions.Count > 0)
        {
            // If multiple hitboxes intersect an enemy on one frame,
            // they will all do damage

            for (int i = 0; i < collisionHandler.collisions.Count; i++)
            {
                CollisionHit hit = collisionHandler.collisions[i];
                // Collision with a hitbox on the other object
                if (hit.otherBox != null && hit.thisBox is Hurtbox && hit.otherBox is Hitbox)
                {
                    {
                        health.Hurt(((Hitbox)hit.otherBox).damage);
                        hurtEffectTimer = hurtEffectTime;
                        // Add points for hitting the target
                        totalMultiplier *= ((Hurtbox)hit.thisBox).scoreMultiplier;
                        totalMultiplier *= ((Hitbox)hit.otherBox).scoreMultiplier;
                        scoreboard.Score += (int)(pointsOnHit * totalMultiplier);
                    }
                    ((Hitbox)hit.otherBox).onHit.Invoke();
                }
            }
        }

        spriteRenderer.enabled = hurtEffectTimer <= 0;

        hurtEffectTimer -= Time.deltaTime;
    }
}
