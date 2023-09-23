using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for moving the background
/// </summary>
public class BackgroundScroll : MonoBehaviour
{
    // SCENE COMPONENTS
    private Camera sceneCamera;
    private Player player;

    // OWN COMPONENTS
    private SpriteRenderer spriteRenderer;

    // VISUAL DATA
    [Tooltip("How many pixels per second the background should move to the left")]
    public float scrollSpeed;
    [Tooltip("Multiplied by player position to offset the background relative to the player")]
    public Vector2 backgroundParallaxMultiplier = Vector2.one;
    private Vector2 playerPos = Vector2.zero;
    private Vector2 parallaxPosition = Vector2.zero;

    private void Awake()
    {
        // Find the camera in the scene
        sceneCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.size = new Vector2(
            (sceneCamera.pixelWidth * 2) + (sceneCamera.pixelWidth * Mathf.Abs(backgroundParallaxMultiplier.x)),
            sceneCamera.pixelHeight + (sceneCamera.pixelHeight * Mathf.Abs(backgroundParallaxMultiplier.y))
            );
        if (player != null)
        {
            playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        }

        // Find new parallax position value
        parallaxPosition = new Vector2(
                (
                    // First get the original position,
                    parallaxPosition.x
                    // Move it left at a rate of scrollSpeed units per second,
                    - scrollSpeed * Time.deltaTime
                )
                // And wrap the background back around if it goes offscreen
                % sceneCamera.pixelWidth,
                0
            );

        transform.position = new Vector3(
                // Take calculated position,
                parallaxPosition.x
                // Offset by half the screen's width so it wraps properly
                + (sceneCamera.pixelWidth / 2)
                // Add Player's X position to the X component
                + playerPos.x * backgroundParallaxMultiplier.x,
                // Set Y component to the Player's y position
                playerPos.y * backgroundParallaxMultiplier.y,
                // Z component set to whatever it was before
                transform.position.z
            );

    }
}
