using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For enemy behavior finite state machine
public enum EnemyState
{
    // Enemy is just entering the stage
    Entering,
    // Enemy is preparing to attack
    AttackWaiting,
    // Enemy is in the process of attacking
    Attacking,
    // Enemy is in the process of recovering after attacking
    Recovering
}

public class Enemy : MonoBehaviour
{
    // SCENE COMPONENTS
    protected GameSessionManager gameSessionManager;
    protected CollisionManager collisionManager;
    protected EnemiesManager enemiesManager;
    protected Scoreboard scoreboard;
    protected Player player;

    // GAMEOBJECT COMPONENTS
    protected SpriteRenderer spriteRenderer;
    protected Movement movement;
    protected CollisionHandler collisionHandler;
    protected Health health;

    // GAMEPLAY VARIABLES
    public EnemyState phase = EnemyState.Entering;
    [Tooltip("The X position on the screen the enemy idles around")]
    public float formationLine = -200f;
    [Tooltip("Points earned on hitting this enemy")]
    public int pointsOnHit = 10;
    [Tooltip("Points earned on killing this enemy")]
    public int pointsOnKill = 100;
    [Tooltip("A tally that gets multiplied to all points this enemy gives, earned from skillful play")]
    public float totalMultiplier = 1f;


    private void Awake()
    {
        // Find collision manager and player in scene
        gameSessionManager = GameObject.FindGameObjectWithTag("GameSessionManager").GetComponent<GameSessionManager>();
        collisionManager = GameObject.FindGameObjectWithTag("CollisionManager").GetComponent<CollisionManager>();
        enemiesManager = GameObject.FindGameObjectWithTag("EnemiesManager").GetComponent<EnemiesManager>();
        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // Get own relevant components
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        movement = gameObject.GetComponent<Movement>();
        collisionHandler = gameObject.GetComponent<CollisionHandler>();
        health = gameObject.GetComponent<Health>();
    }

    private void Start()
    {
        // Add own collision handler to the manager
        collisionManager.AddToColliderSets(collisionHandler);
        SpawnBehavior();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void SpawnBehavior()
    {

    }

    // Enemies don't do anything special when their hitboxes intercept a player by default
    public void OnHit()
    {

    }

    public void OnDeath()
    {
        scoreboard.Score += (int)(pointsOnKill * totalMultiplier);
        collisionManager.RemoveFromColliderSets(collisionHandler);
        enemiesManager.EnemyDied(this);

        Destroy(gameObject);
    }

    public void NewGame()
    {
        collisionManager.RemoveFromColliderSets(collisionHandler);
        Destroy(gameObject);
    }
}
