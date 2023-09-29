using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
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
        if (gameSessionManager.gameSessionRunning)
        {
            
        }

    }
}
