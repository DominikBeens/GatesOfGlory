using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{

    public static GameManager instance;

    public enum GameState
    {
        Cinematic,
        Playing,
        Paused
    }
    public GameState gameState;

    public bool showStartGameAnimation = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (UIManager.instance != null)
            {
                UIManager.instance.PauseButton();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTimeScale();
        }
    }

    private void ToggleTimeScale()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 4;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
