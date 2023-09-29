using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private GameSessionManager gameSessionManager;
    private Player player;

    [Tooltip("The amount of time the controller should pause between spawning enemies")]
    public float enemyPause = 3f;
    private float enemyTimer = 0f;
    [Tooltip("The amount different pause times can vary from each other")]
    public float timerVariation = 1.5f;

    // A list of living enemies
    public List<Enemy> enemies;

    // The number of enemies that can be alive at once;
    // is changed throughout the game
    [Tooltip("The number of enemies that can be on screen at a given time")]
    public float maxEnemiesInitial = 1.0f;
    private float maxEnemies;
    [Tooltip("Changes maxEnemies every time an enemy spawns")]
    public float maxEnemiesChangeInitial = 1.1f;
    private float maxEnemiesChange;
    [Tooltip("Changes maxEnemiesChange every time an enemy spawns")]
    public float maxEnemiesChangeSquared = -0.1f;

    public List<Enemy> enemyTypes;
    // A list of weights for spawning each type of enemy
    public List<int> enemyWeights;
    private int weightTotal;
    // Variables for deciding new enemies
    float enemyToSpawn;
    private Enemy newEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        gameSessionManager = GameObject.FindGameObjectWithTag("GameSessionManager").GetComponent<GameSessionManager>();

        maxEnemies = maxEnemiesInitial;
        maxEnemiesChange = maxEnemiesChangeInitial;

        enemies = new List<Enemy>();

        foreach (int weight in enemyWeights)
        {
            weightTotal += weight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If there is a game running,
        if (gameSessionManager.gameSessionRunning)
        {
            // If no reference to player, that means this is a new game
            if (player = null)
            {
                // Get player from game session manager
                player = gameSessionManager.player;

                // Delete any current enemies
                for (int i = 0; i < enemies.Count; i++)
                {
                    Enemy enemy = enemies[i];
                    enemies.Remove(enemy);
                    enemy.NewGame();
                }
            }

            // If enemy timer is up,
            if (enemyTimer <= 0)
            {
                // Only spawn enemy if not at max enemy count
                if (enemies.Count <= (int)maxEnemies - 1)
                {
                    // Get a random value
                    enemyToSpawn = Random.value;

                    // Pick an enemy based on that random value
                    if (enemyToSpawn < ((float)enemyWeights[0] / weightTotal))
                    {
                        newEnemy = Instantiate<Enemy>(enemyTypes[0]);
                        newEnemy.formationLine = Random.Range(-220f, -160f);
                    }
                    else
                    {
                        newEnemy = Instantiate<Enemy>(enemyTypes[1]);
                        newEnemy.formationLine = Random.Range(-120f, -60f);
                    }

                    // Set its starting position randomly
                    newEnemy.transform.position = new Vector3(
                        Random.Range(-700f, -300f),
                        Random.Range(-140f, 140f),
                        0);
                    // Initialize the enemy at that position
                    newEnemy.SpawnBehavior();
                    enemies.Add(newEnemy.GetComponent<Enemy>());
                }

                // Add a somewhat random amount of time to the timer
                enemyTimer = enemyPause + (Random.value * timerVariation) - (timerVariation * 0.5f);
            }

            enemyTimer -= Time.deltaTime;
        }
    }

    public void EnemyDied(Enemy enemy)
    {
        maxEnemiesChange = Mathf.Max(maxEnemiesChange + maxEnemiesChangeSquared, 0.0f);
        maxEnemies += maxEnemiesChange;

        enemies.Remove(enemy);
    }
}
