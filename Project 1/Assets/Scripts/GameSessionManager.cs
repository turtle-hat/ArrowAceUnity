using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Handles starting new games as well as player input
public class GameSessionManager : MonoBehaviour
{
    public EnemiesManager enemiesManager;

    public GameObject playerObject;

    // Reference to own input component
    private PlayerInput playerInput;

    // Public reference to the current player
    //[HideInInspector]
    public Player player;

    [HideInInspector]
    public bool gameSessionRunning;
    [HideInInspector]
    public bool gameStarted;

    private void Awake()
    {
        gameStarted = false;
        gameSessionRunning = false;
        playerInput = gameObject.GetComponent<PlayerInput>();
        enemiesManager = GameObject.FindGameObjectWithTag("EnemiesManager").GetComponent<EnemiesManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Runs when the user starts a game
    public void OnGameStart(InputAction.CallbackContext startContext)
    {
        // If button is down
        if (startContext.ReadValueAsButton())
        {
            gameStarted = true;

            if (gameSessionRunning == true)
            {
                player.OnDeath();
            }

            // Kill current enemies
            enemiesManager.NewGame();

            // Reset score
            Scoreboard scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();
            scoreboard.NewGame();

            // Center player on screen and display them
            player.NewGame();

            // Start the game session
            gameSessionRunning = true;
        }
    }
}
