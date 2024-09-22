using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // SCENE COMPONENTS
    // The object in the scene that allows objects to track their own collisions
    private CollisionManager collisionManager;
    private GameSessionManager gameSessionManager;
    private Slider userInterfaceHealthbar;

    // GAMEOBJECT COMPONENTS
    // The component for displaying the sprite
    private SpriteRenderer spriteRenderer;
    // The component for moving the Player
    private Movement movement;
    // The component that stores all the Player's collision boxes
    private CollisionHandler collisionHandler;
    // The component dealing with the Player's health
    private Health health;

    // INPUT
    // Moving
    private Vector2 movementInput;
    // Whether the Fire button is pressed
    private bool isFiring;

    // GAMEPLAY
    [HideInInspector]
    public int score;
    [Tooltip("How long in seconds after getting hit the object should be unable to be hurt")]
    public float invincibilityLength = 1f;
    // If this number is positive, the player will not register any damage
    private float invincibilityTimer = 0f;
    [Tooltip("How long in seconds after firing an arrow the player must wait until firing again")]
    public float fireRecoveryLength = 0.4f;
    // If this number is positive the player will not fire when the fire button is pressed
    private float fireRecoveryTimer = 0f;
    [Tooltip("The weapons the Player can use")]
    public List<GameObject> projectiles;
    // Indices of weapons the player can use
    public int defaultWeapon = 0;
    private int currentWeapon;


    private void Awake()
    {
        // Find necessary components in scene
        // Method of finding components in a scene found here: https://answers.unity.com/questions/15801/finding-cameras.html
        collisionManager = GameObject.FindGameObjectWithTag("CollisionManager").GetComponent<CollisionManager>();
        gameSessionManager = GameObject.FindGameObjectWithTag("GameSessionManager").GetComponent<GameSessionManager>();
        userInterfaceHealthbar = GameObject.FindGameObjectWithTag("Healthbar").GetComponent<Slider>();

        // Find necessary components in own object
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        movement = gameObject.GetComponent<Movement>();
        collisionHandler = gameObject.GetComponent<CollisionHandler>();
        health = gameObject.GetComponent<Health>();

        isFiring = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize Collision Manager
        collisionManager.AddToColliderSets(collisionHandler);
    }

    // Called when a game starts
    public void NewGame()
    {
        // Reset position, health, input, and timers
        transform.position = new Vector3(150f, 0f, 0f);
        health.health = 1f;
        isFiring = false;
        invincibilityTimer = 0f;
        fireRecoveryTimer = 0f;

        // Show sprite
        spriteRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Only do anything if the game session has been started
        if (gameSessionManager.gameSessionRunning)
        {
            // Move
            movement.Move(movementInput);

            // Move the Player back onscreen if it's offscreen
            foreach (PhysicsBox phys in collisionHandler.physicsBoxes)
            {
                transform.position += phys.GetDistanceOffscreen();
            }

            if (invincibilityTimer > 0)
            {
                spriteRenderer.enabled = (int)(invincibilityTimer * 10) % 2 == 1;
            }
            else
            {
                if (!spriteRenderer.enabled && invincibilityTimer < 0)
                {
                    spriteRenderer.enabled = true;
                }
            }

            // Reset collisions and then get relevant collisions
            collisionHandler.ResetCollisions();
            collisionManager.CheckCollisionsOnHandler(collisionHandler);

            // If it registers collisions...
            if (collisionHandler.collisions.Count > 0)
            {
                // If multiple hitboxes intersect the player on this frame,
                // this variable will only make them take damage once from the most damaging one
                float highestDamage = 0;

                for (int i = 0; i < collisionHandler.collisions.Count; i++)
                {
                    CollisionHit hit = collisionHandler.collisions[i];

                    // If the player's catch box is colliding with an enemy projectile,
                    // set it as the player's current weapon
                    if (hit.otherBox != null && hit.otherBox is Hurtbox && hit.otherHandler.collisionClass == CollisionClass.EnemyProjectile)
                    {
                        currentWeapon = 1;
                        collisionManager.RemoveFromColliderSets(hit.otherHandler);
                        Destroy(hit.otherBox.gameObject);
                    }

                    // Collision with a hitbox on the other object
                    if (hit.otherBox != null && hit.otherBox is Hitbox)
                    {
                        // If not invincible, add that damage to the highest damage
                        if (invincibilityTimer <= 0)
                        {
                            highestDamage = Mathf.Max(highestDamage, ((Hitbox)hit.otherBox).damage);
                        }
                        ((Hitbox)hit.otherBox).onHit.Invoke();
                    }
                }

                // If the player took any damage, apply damage and add invincibility frames
                if (highestDamage > 0)
                {
                    health.Hurt(highestDamage);
                    invincibilityTimer = invincibilityLength;
                }
            }

            userInterfaceHealthbar.value = health.health;

            // If able to fire and pressing the button to fire, Instantiate a new projectile
            if (fireRecoveryTimer <= 0f && isFiring)
            {
                //Debug.Log($"Blamm! Directional Input: {directionInput}");

                Projectile newProjectile = Instantiate(projectiles[currentWeapon]).GetComponent<Projectile>();

                // Set the projectile's position, direction, and acceleration/damping (which should both be 0)
                newProjectile.SetMotion(
                    new Vector3(
                        transform.position.x - 24,
                        transform.position.y,
                        transform.position.z + 1),
                    new Vector2(-1f, 0f),
                    0f, 0f
                    );

                if (currentWeapon != defaultWeapon)
                {
                    currentWeapon = defaultWeapon;
                }

                fireRecoveryTimer = fireRecoveryLength;
            }

            // Adjust timers
            invincibilityTimer -= Time.deltaTime;
            fireRecoveryTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Get inputs from InputActions and calls Move script
    /// </summary>
    /// <param name="moveContext"></param>
    public void OnMove(InputAction.CallbackContext moveContext)
    {
        // Get movement direction from input
        movementInput = moveContext.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext fireContext)
    {
        isFiring = fireContext.ReadValueAsButton();
    }

    public void OnDeath()
    {
        // Hide sprite
        spriteRenderer.enabled = false;

        // Stop game session
        gameSessionManager.gameSessionRunning = false;
    }
}
