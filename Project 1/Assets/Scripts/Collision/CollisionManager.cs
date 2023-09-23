using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionBoxType
{
    // This's physics boxes hitting others' physics boxes
    Physics,
    // This's hitboxes hitting others' hurtboxes
    HitToHurt,
    // This's hurtboxes hitting others' hitboxes
    HurtToHit,
}

/// <summary>
/// Checks collisions between objects
/// </summary>
public class CollisionManager : MonoBehaviour
{
    [HideInInspector]
    // Stores all relevant collisionHandlers
    private List<CollisionHandler> colliders = new List<CollisionHandler>();
    // For internal use, stores colliders of different collision classes in one data structure
    private Dictionary<CollisionClass, List<CollisionHandler>> collidersByType =
        new Dictionary<CollisionClass, List<CollisionHandler>>();

    public List<CollisionHandler> Colliders
    {
        get { return colliders; }
    }

    // Awake is called before Start
    void Awake()
    {
        // Help with looping through enums obtained from here
        // https://learn.microsoft.com/en-us/dotnet/api/system.enum.getvalues

        // Loop through the CollisionClass enum
        foreach (int i in Enum.GetValues(typeof(CollisionClass)))
        {
            // Add a dictionary item linking each CollisionClass value to its own dedicated set
            collidersByType.Add(
                (CollisionClass)i,
                new List<CollisionHandler>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToColliderSets(CollisionHandler handler)
    {
        // Adds to the comprehensive set of all colliders
        colliders.Add(handler);
        // Adds to a subset based on what collisions it handles
        collidersByType[handler.collisionClass].Add(handler);
        //Debug.Log($"{handler.gameObject.name} has been added to {handler.collisionClass} set");
    }

    public void RemoveFromColliderSets(CollisionHandler handler)
    {
        if (colliders.Contains(handler))
        {
            colliders.Remove(handler);
        }
        if (collidersByType[handler.collisionClass].Contains(handler))
        {
            collidersByType[handler.collisionClass].Remove(handler);
        }
    }

    /// <summary>
    /// Check for and register collisions on the specified collision handler
    /// </summary>
    /// <param name="focusedHandler">The collision handler to have collisions checked for</param>
    public void CheckCollisionsOnHandler(CollisionHandler focusedHandler)
    {
        // Player collisions
        if (focusedHandler.collisionClass == CollisionClass.Player)
        {
            // Players collide with walls
            SearchForCollisions(focusedHandler, CollisionClass.Solid, CollisionBoxType.Physics);
            // Players can be hurt by Enemies but can't hit them
            SearchForCollisions(focusedHandler, CollisionClass.Enemy, CollisionBoxType.HurtToHit);
            // Players can be hurt by Enemy Projectiles but can also hit them to steal them
            SearchForCollisions(focusedHandler, CollisionClass.EnemyProjectile, CollisionBoxType.HurtToHit);
            SearchForCollisions(focusedHandler, CollisionClass.EnemyProjectile, CollisionBoxType.HitToHurt);
        }

        // Enemy collisions
        if (focusedHandler.collisionClass == CollisionClass.Enemy)
        {
            // Enemies collide with walls
            SearchForCollisions(focusedHandler, CollisionClass.Solid, CollisionBoxType.Physics);
            // Enemies can hit Players but can't be hurt by them
            SearchForCollisions(focusedHandler, CollisionClass.Player, CollisionBoxType.HitToHurt);
            // Enemies can be hurt by Player Projectiles but can't hit them
            SearchForCollisions(focusedHandler, CollisionClass.PlayerProjectile, CollisionBoxType.HurtToHit);
        }

        // Projectiles don't track their own hurt/hit collisions,
        // instead relying on what they're colliding with to do that for them

        // Player Projectile collisions
        if (focusedHandler.collisionClass == CollisionClass.PlayerProjectile)
        {
            SearchForCollisions(focusedHandler, CollisionClass.Solid, CollisionBoxType.Physics);
        }

        // Enemy Projectile collisions
        if (focusedHandler.collisionClass == CollisionClass.EnemyProjectile)
        {
            SearchForCollisions(focusedHandler, CollisionClass.Solid, CollisionBoxType.Physics);
        }
    }

    private void SearchForCollisions(CollisionHandler focusedHandler, CollisionClass otherClass, CollisionBoxType boxType)
    {
        // This's physboxes and all other Solids' physboxes
        foreach (CollisionHandler otherHandler in collidersByType[otherClass])
        {
            // This's physics boxes hitting others' physics boxes
            if (boxType == CollisionBoxType.Physics)
            {
                // Loop through all physboxes in focused
                foreach (PhysicsBox focusedPhys in focusedHandler.physicsBoxes)
                {
                    // Loop through all physboxes in other
                    foreach (PhysicsBox otherPhys in otherHandler.physicsBoxes)
                    {
                        // If the current 
                        if (focusedPhys.IsColliding(otherPhys))
                        {
                            focusedHandler.RegisterCollision(focusedPhys, otherPhys, otherHandler);
                        }
                    }
                }
            }
            // This's hitboxes hitting others' hurtboxes
            else if (boxType == CollisionBoxType.HitToHurt)
            {
                foreach (Hitbox focusedHit in focusedHandler.hitboxes)
                {
                    foreach (Hurtbox otherHurt in otherHandler.hurtboxes)
                    { 
                        if (focusedHit.IsColliding(otherHurt))
                        {
                            focusedHandler.RegisterCollision(focusedHit, otherHurt, otherHandler);
                        }
                    }
                }
            }
            // This's hurtboxes hitting others' hitboxes
            else if (boxType == CollisionBoxType.HurtToHit)
            {
                foreach (Hurtbox focusedHurt in focusedHandler.hurtboxes)
                {
                    foreach (Hitbox otherHit in otherHandler.hitboxes)
                    {
                        if (focusedHurt.IsColliding(otherHit))
                        {
                            focusedHandler.RegisterCollision(focusedHurt, otherHit, otherHandler);
                        }
                    }
                }
            }
        }
    }
}
