using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionClass
{
    Solid,
    Player,
    Enemy,
    PlayerProjectile,
    EnemyProjectile
}

/// <summary>
/// Stores collisions on an individual object's hitboxes
/// </summary>
public class CollisionHandler : MonoBehaviour
{
    [Tooltip("The type of object this is considered in the game. Used to determine which other objects it collides with.")]
    public CollisionClass collisionClass = CollisionClass.Solid;
    [Tooltip("Collision boxes defining the bounds of the object, used for colliding with objects")]
    public List<PhysicsBox> physicsBoxes = new List<PhysicsBox>();
    [Tooltip("Collision boxes that define where the object can be hurt")]
    public List<Hurtbox> hurtboxes = new List<Hurtbox>();
    [Tooltip("Collision boxes that define a part of the object that can hurt other objects")]
    public List<Hitbox> hitboxes = new List<Hitbox>();
    // A list of all the collision boxes this has collided with in the past frame
    //[HideInInspector]
    public List<CollisionHit> collisions = new List<CollisionHit>();

    // Start is called before the first frame update
    void Start()
    {
        ResetCollisions();
    }

    [Tooltip("Registers a collision in this collision handler's list of box collisions.")]
    /// <summary>
    /// Registers a collision in this collision handler's list
    /// </summary>
    /// <param name="collidee">The collision box in this CollisionHandler which was collided into</param>
    /// <param name="collider">The other collision box colliding into this one</param>
    /// <param name="colliderHandler">The CollisionHandler of the other collision box colliding with this one</param>
    /// <returns>The new collision between the two</returns>
    public CollisionHit RegisterCollision(CollisionBox collidee, CollisionBox collider, CollisionHandler colliderHandler)
    {
        CollisionHit newCollision = new CollisionHit(this, colliderHandler,
            collidee, collider, collidee.FindCollisionDistance(collider));
        collisions.Add(newCollision);
        return newCollision;
    }

    public void ResetCollisions()
    {
        collisions.Clear();
    }

    /// <summary>
    /// Draw all collision boxes
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (PhysicsBox physicsBox in physicsBoxes)
        {
            Gizmos.DrawWireCube(transform.position + physicsBox.Offset3, physicsBox.Dimensions3);
            Gizmos.DrawLine(physicsBox.MinPoint, physicsBox.MaxPoint);
        }
        Gizmos.color = Color.green;
        foreach (Hurtbox hurtbox in hurtboxes)
        {
            Gizmos.DrawWireCube(transform.position + hurtbox.Offset3, hurtbox.Dimensions3);
            Gizmos.DrawLine(hurtbox.MinPoint, hurtbox.MaxPoint);
        }
        Gizmos.color = Color.red;
        foreach (Hitbox hitbox in hitboxes)
        {
            Gizmos.DrawWireCube(transform.position + hitbox.Offset3, hitbox.Dimensions3);
            Gizmos.DrawLine(hitbox.MinPoint, hitbox.MaxPoint);
        }
        Gizmos.color = Color.yellow;
        foreach (CollisionHit hit in collisions)
        {

            if (hit.otherBox != null)
            {
                Gizmos.DrawLine(hit.thisBox.Center, hit.otherBox.Center);
            }
        }
    }
}
