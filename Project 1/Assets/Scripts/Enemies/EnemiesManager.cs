using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    Player player;

    [Tooltip("The amount of time the controller should pause between spawning enemies")]
    public float enemyPause = 3f;
    private float enemyTimer = 0f;
    [Tooltip("The amount different pause times can vary from each other")]
    public float timerVariation = 1.5f;
    public List<Enemy> enemies;
    // A list of weights for spawning each type of enemy
    public List<int> enemyWeights;
    private int weightTotal;
    // Variables for deciding new enemies
    float enemyToSpawn;
    Enemy newEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        foreach(int weight in enemyWeights)
        {
            weightTotal += weight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If timer is up,
        if (player != null && enemyTimer <= 0)
        {
            // Get a random value
            enemyToSpawn = Random.value;

            // Pick an enemy based on that random value
            if (enemyToSpawn < ((float)enemyWeights[0] / weightTotal))
            {
                newEnemy = Instantiate<Enemy>(enemies[0]);
                newEnemy.formationLine = Random.Range(-220f, -160f);
            }
            else
            {
                newEnemy = Instantiate<Enemy>(enemies[1]);
                newEnemy.formationLine = Random.Range(-120f, -60f);
            }

            // Set its starting position randomly
            newEnemy.transform.position = new Vector3(
                Random.Range(-700f, -300f),
                Random.Range(-140f, 140f),
                0);
            // Initialize the enemy at that position
            newEnemy.SpawnBehavior();

            // Add a somewhat random amount of time to the timer
            enemyTimer = enemyPause + (Random.value * timerVariation) - (timerVariation * 0.5f);
        }

        enemyTimer -= Time.deltaTime;
    }
}
