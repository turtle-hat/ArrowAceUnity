using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collision boxes that define a region the object can be hurt in if entered by a hitbox
/// </summary>
public class Hurtbox : CollisionBox
{
    [Tooltip("Value multiplied to all damage taken on this box")]
    public float damageMultiplier = 1;
    [Tooltip("How much is multiplied to the score when hitting this hitbox")]
    public float scoreMultiplier = 1f;
}
