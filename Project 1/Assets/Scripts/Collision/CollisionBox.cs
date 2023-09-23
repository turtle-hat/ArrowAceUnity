using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collision boxes define the bounds of some part of an object
/// </summary>
public class CollisionBox : MonoBehaviour
{
    // Name of the box, for organizational purposes
    public string regionName = "Generic";
    // Collision info
    [Tooltip("How far the sides of the collision box extend")]
    public Vector2 dimensions;
    [Tooltip("How far relative to the sprite to offset the collision box")]
    public Vector2 relativeOffset;

    // Vector3 properties
    public Vector3 Dimensions3
    {
        get { return new Vector3(dimensions.x, dimensions.y, 0); }
    }



    // Collision box info relative to the object, without factoring in rotation

    public Vector3 RelativeOffset3
    {
        get { return new Vector3(relativeOffset.x, relativeOffset.y, 0); }
    }

    // Collision box center is found from the parent's position and the bounding box's offset
    public Vector2 RelativeCenter
    {
        get
        {
            return new Vector2(
                transform.position.x + relativeOffset.x,
                transform.position.y + relativeOffset.y
            );
        }
    }
    // Collision box corners are offset by the bounding box's offset from the sprite's position,
    // found by adding/subtracting half the box's dimensions,
    // then rounded to an integer (to match what's seen onscreen)
    public Vector2 RelativeMinPoint
    {
        get
        {
            return new Vector2(
                Mathf.Round(RelativeCenter.x - (dimensions.x / 2)),
                Mathf.Round(RelativeCenter.y - (dimensions.y / 2))
            );
        }
    }
    public Vector2 RelativeMaxPoint
    {
        get
        {
            return new Vector2(
                Mathf.Round(RelativeCenter.x + (dimensions.x / 2)),
                Mathf.Round(RelativeCenter.y + (dimensions.y / 2))
            );
        }
    }

    public float RelativeLeftEdge
    {
        get { return RelativeMinPoint.x; }
    }
    public float RelativeRightEdge
    {
        get { return RelativeMaxPoint.x; }
    }
    public float RelativeBottomEdge
    {
        get { return RelativeMinPoint.y; }
    }
    public float RelativeTopEdge
    {
        get { return RelativeMaxPoint.y; }
    }



    // Collision box info with the object's rotation factored in
    /* 
     * NOTE: I intended the rotatable to be used for projectiles that needed to face multiple directions,
     * but I forgot 
     */

    [Tooltip("The coordinates of the center of the collision box relative to the sprite when rotated")]
    public Vector2 Offset
    {
        // Method for rotating a Vector3 obtained from here:
        // https://gamedevbeginner.com/how-to-rotate-in-unity-complete-beginners-guide/#rotate_vector
        get { return Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * relativeOffset; }
    }
    public Vector3 Offset3
    {
        get { return new Vector3(Offset.x, Offset.y, 0); }
    }

    public Vector2 Center
    {
        get
        {
            return new Vector2(
                transform.position.x + Offset.x,
                transform.position.y + Offset.y
            );
        }
    }
    public Vector2 MinPoint
    {
        get {
            return new Vector2(
                Mathf.Round(Center.x - (dimensions.x / 2)),
                Mathf.Round(Center.y - (dimensions.y / 2))
            );
        }
    }
    public Vector2 MaxPoint
    {
        get
        {
            return new Vector2(
                Mathf.Round(Center.x + (dimensions.x / 2)),
                Mathf.Round(Center.y + (dimensions.y / 2))
            );
        }
    }

    public float LeftEdge
    {
        get { return MinPoint.x; }
    }
    public float RightEdge
    {
        get { return MaxPoint.x; }
    }
    public float BottomEdge
    {
        get { return MinPoint.y; }
    }
    public float TopEdge
    {
        get { return MaxPoint.y; }
    }

    /// <summary>
    /// Finds how far another collision box is situated inside this collision box
    /// </summary>
    /// <param name="other">The other collision box being checked for a collision</param>
    /// <returns>The distance from the colliding edge(s) of this bounding box to the colliding edge(s) of the other bounding box</returns>
    public Vector2 FindCollisionDistance(CollisionBox other)
    {
        Vector2 edgeDistance = Vector2.zero;

        // X
        // If my box's center is greater than the other's, find distance between its higher edge and my lower edge
        if (Center.x > other.Center.x)
        {
            edgeDistance += new Vector2(other.RightEdge - LeftEdge, 0);
        }
        // If my box's center is less than the other's, find distance between its lower edge and my higher edge
        else
        {
            edgeDistance += new Vector2(other.LeftEdge - RightEdge, 0);
        }

        // Y
        if (Center.y > other.Center.y)
        {
            edgeDistance += new Vector2(0, other.TopEdge - BottomEdge);
        }
        else
        {
            edgeDistance += new Vector2(0, other.BottomEdge - TopEdge);
        }

        return edgeDistance;
    }

    /// <summary>
    /// Uses AABB collision to check whether two bounding boxes intersect
    /// </summary>
    /// <param name="other">The other collision box being checked for a collision</param>
    /// <returns>true if colliding, false if not colliding</returns>
    public bool IsColliding(CollisionBox other)
    {
        return
            RightEdge > other.LeftEdge &&
            TopEdge > other.BottomEdge &&
            other.RightEdge > LeftEdge &&
            other.TopEdge > BottomEdge;
    }
}
