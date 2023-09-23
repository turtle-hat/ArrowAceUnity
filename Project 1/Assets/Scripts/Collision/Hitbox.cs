using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collision boxes that define a region of the object that can hurt other objects in by entering their hitbox
/// </summary>
public class Hitbox : CollisionBox
{
    [Tooltip("How much damage this hitbox deals")]
    public float damage = 0.1f;
    [Tooltip("How much is multiplied to the score by using this hitbox")]
    public float scoreMultiplier = 1f;
    [Tooltip("The event to be called when this hitbox deals damage")]
    public UnityEvent onHit;
}
