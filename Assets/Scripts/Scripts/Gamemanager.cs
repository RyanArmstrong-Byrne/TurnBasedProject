using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject pauseMenu;
    public GameStates state = GameStates.PlayerTurn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(this);
        }

        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (state == GameStates.Pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        state = GameStates.Pause;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        state = GameStates.PlayerTurn;
    }
}

public enum GameStates
{
    PlayerTurn, EnemyTurn, Pause, Menu, Win, Lose, Battle
}
