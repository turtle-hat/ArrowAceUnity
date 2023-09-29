using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    private Text text;
    [Range(0, 9999999)]
    private int score = 0;
    // Score can only ever be increased
    public int Score
    {
        get { return score; }
        set { score = Mathf.Max(score, value); }
    }

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"{score.ToString("0000000")}";
    }

    public void NewGame()
    {
        score = 0;
    }
}
