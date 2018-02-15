using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

    public static GameManager instance;

    public enum GameState
    {
        Playing,
        Paused
    }
    public GameState gameState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
