using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSessionManager : MonoBehaviour
{
    public GameObject playerObject;
    // Public reference to the current player
    public Player player;

    [HideInInspector]
    public bool gameSessionRunning;

    private void Awake()
    {
        gameSessionRunning = false;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Runs when the user starts a game
    public void OnGameStart(InputAction.CallbackContext startContext)
    {
        // Reset score
        Scoreboard scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();
        scoreboard.NewGame();

        // Create new player in center of screen
        GameObject newPlayer = Instantiate(playerObject);
        newPlayer.transform.position = new Vector3(0, 0, 0);

        // Get a reference to the player and start the game session
        player = newPlayer.GetComponent<Player>();
        gameSessionRunning = true;
    }
}
