using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject slider;

    private Spawner spawner;

    public GameObject battery;
    public GameObject score;
    public TMP_Text finalScore;

    void Start(){
        gameOverUI.SetActive(false);
    }

    // Call this method when the game ends
    public void ShowGameOver() {
        //spawner.enabled = false;
        gameOverUI.SetActive(true);
        slider.SetActive(false);
        int endScore = score.GetComponent<ScoreCounter>().finalScore;
        Debug.Log(endScore);
        finalScore.text = "Final Score: " + endScore;
    }

    public void RestartGame() {
        Time.timeScale = 1;
        slider.SetActive(true); // put the battery back on screen
        battery.GetComponent<DrainBattery>().Reset(); // reset battery
        // spawner.enabled = true; // stop enemy spawns
        score.GetComponent<ScoreCounter>().Reset(); // reset score
        gameOverUI.SetActive(false); // take away the Gameover UI
    }

    public void QuitGame() {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
