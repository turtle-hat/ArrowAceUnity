using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpear : Enemy
{
    [Tooltip("The amount of time the enemy waits between firing spears at the player")]
    public float attackPause = 2f;
    private float attackTimer = 0f;
    [Tooltip("Position the enemy starts at")]
    public Vector2 startPosition = Vector2.zero;
    [Tooltip("The projectile this enemy fires")]
    public GameObject projectile;
    private float hurtEffectTime = 0.1f;
    private float hurtEffectTimer = 0f;

    // An overridable method called within Enemy.Start() for initializing stuff
    public override void SpawnBehavior()
    {
        movement.AccelerationTime = 0f;
        movement.DampingTime = 0.9f;
        movement.TopSpeed = 80f;
        movement.velocity = new Vector2(movement.TopSpeed, 0);
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case EnemyState.Entering:
                if ((startPosition.x < formationLine && movement.EndPosition.x <= formationLine) ||
                    (startPosition.x > formationLine && movement.EndPosition.x >= formationLine))
                {
                    movement.Move(new Vector2(Mathf.Sign(formationLine - startPosition.x), 0));
                }
                else
                {
                    movement.Move(Vector2.zero);
                    phase = EnemyState.AttackWaiting;
                }
                break;
            case EnemyState.AttackWaiting:
                movement.Move(Vector2.zero);
                attackTimer -= Time.deltaTime;

                if (player != null && attackTimer <= 0)
                {
                    attackTimer = attackPause;
                    phase = EnemyState.Attacking;
                }
                break;
            case EnemyState.Attacking:
                Projectile newProjectile = Instantiate<GameObject>(projectile).GetComponent<Projectile>();
                newProjectile.SetMotion(
                new Vector3(
                    transform.position.x + 32,
                    transform.position.y,
                    transform.position.z),
                new Vector2(1f, 0f),
                0f, 0f
                );
                phase = EnemyState.AttackWaiting;
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
