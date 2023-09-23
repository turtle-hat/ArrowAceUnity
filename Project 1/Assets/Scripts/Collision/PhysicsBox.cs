using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collision boxes that define a solid region of the object that cannot be passed through
/// </summary>
public class PhysicsBox : CollisionBox
{
    // Screen corner positions, rounded to the pixel grid
    private Camera sceneCamera;
    private Vector3 screenCornerBL;
    private Vector3 screenCornerUR;

    private void Awake()
    {
        sceneCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // Get dimensions of screen and store in two variables
        screenCornerBL = sceneCamera.ScreenToWorldPoint(
            new Vector3(0, 0, sceneCamera.nearClipPlane));
        screenCornerUR = sceneCamera.ScreenToWorldPoint(
            new Vector3(sceneCamera.pixelWidth, sceneCamera.pixelHeight, sceneCamera.nearClipPlane));
        //Debug.Log(screenCornerUR);
        //Debug.Log(screenCornerBL);
    }

    /// <summary>
    /// Check if this hitbox is offscreen and returns how far the hitbox stretches offscreen
    /// </summary>
    /// <returns>How far the hitbox extends off the side of the screen</returns>
    public Vector3 GetDistanceOffscreen()
    {
        Vector3 distanceOffscreen = Vector3.zero;

        // X
        if (RightEdge > screenCornerUR.x)
        {
            distanceOffscreen += new Vector3(screenCornerUR.x - RightEdge, 0, 0);
        }
        else if (LeftEdge < screenCornerBL.x)
        {
            distanceOffscreen += new Vector3(screenCornerBL.x - LeftEdge, 0, 0);
        }
        // Y
        if (TopEdge > screenCornerUR.y)
        {
            distanceOffscreen += new Vector3(0, screenCornerUR.y - TopEdge, 0);
        }
        else if (BottomEdge < screenCornerBL.y)
        {
            distanceOffscreen += new Vector3(0, screenCornerBL.y - BottomEdge, 0);
        }

        return distanceOffscreen;
    }
}
