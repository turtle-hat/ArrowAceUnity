using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("Stores a collision between two collision boxes, including information about the hit")]
public class CollisionHit
{
    // The collision box which registered the hit
    public CollisionBox thisBox;
    // The other collision box colliding into the registering box
    public CollisionBox otherBox;
    // The collision classes of each Collision Handler involved in the colliison
    public CollisionHandler thisHandler;
    public CollisionHandler otherHandler;
    // The distance from the other collision box's edge
    public Vector2 intersectionDistance;

    public CollisionHit(CollisionHandler intersectedHandler, CollisionHandler intersectingHandler,
         CollisionBox intersectedBox, CollisionBox intersectingBox, Vector2 intersectionDistance)
    {
        this.thisHandler = intersectedHandler;
        this.otherHandler = intersectingHandler;
        this.thisBox = intersectedBox;
        this.otherBox = intersectingBox;
        this.intersectionDistance = intersectionDistance;
    }
}
