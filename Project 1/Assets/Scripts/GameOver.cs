using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private GameSessionManager gameSessionManager;
    private RectTransform rectTransform;

    private void Awake()
    {
        gameSessionManager = GameObject.FindGameObjectWithTag("GameSessionManager").GetComponent<GameSessionManager>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        //rectTransform.position = new Vector3(240, 540, 0);
        transform.position = new Vector3(0, 360, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSessionManager.gameStarted && !gameSessionManager.gameSessionRunning)
        {
            //if (rectTransform.position.y > 180)
            //{
            //    rectTransform.position = new Vector3(240, rectTransform.position.y - 160 * Time.deltaTime, 0);
            //}
            //if (rectTransform.position.y < 180)
            //{
            //    rectTransform.position = new Vector3(240, 180, 0);
            //}
            if (transform.position.y > 0)
            {
                transform.position = new Vector3(0, rectTransform.position.y - 160 * Time.deltaTime, 0);
            }
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(0, 0, 0);
            }
        }
        else
        {
            transform.position = new Vector3(0, 360, 0);
        }
    }
}
