using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score = 0;
    public TMP_Text finalScore;

    // Start is called before the first frame update
    void Start()
    {
        Update();
    }

    public void Reset()
    {
        Debug.Log("score was reset");
        scoreText.text = "Score: " + 0;
        score = 0;
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
    }

    // points can be changed depending on what enemy you kill, or not
    public void AddScore(int points)
    {
        score += points; // Increase the score
        Update(); // Refresh the UI
    }

    public void SetFinalScore()
    {
        finalScore.text = "Final Score: " + score;
    }          
}
