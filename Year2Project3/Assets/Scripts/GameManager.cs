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

    public enum PlayerState
    {
        Idle,
        UsingWeapon
    }
    public PlayerState playerState;

    private CastleWeapon weaponInUse;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartUsingWeapon(CastleWeapon weapon)
    {
        weaponInUse = weapon;
        playerState = PlayerState.UsingWeapon;
    }

    public void StopUsingWeapon()
    {
        weaponInUse = null;
        playerState = PlayerState.Idle;
    }
}
