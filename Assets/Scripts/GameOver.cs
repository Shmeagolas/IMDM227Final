using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject slider;

    public GameObject battery;

    void Start(){
        gameOverUI.SetActive(false);
    }

    // Call this method when the game ends
    public void ShowGameOver() {
        gameOverUI.SetActive(true);
        slider.SetActive(false);
    }

    public void RestartGame() {
        Time.timeScale = 1;
        slider.SetActive(true);
        battery.GetComponent<DrainBattery>().Reset();
        gameOverUI.SetActive(false);
    }

    public void QuitGame() {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
