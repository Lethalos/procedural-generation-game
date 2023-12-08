using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Idle,
    Active,
    Info,

}

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameState currentGameState;
    

    public static event Action<GameState> OnGameStateChanged;
    bool inputDetected = false;
    private void Start()
    {
        currentGameState = GameState.Idle;
       
        
    }

    public void ChangeGameState(GameState newGameState)
    {
        // Perform any necessary actions based on the current and new game states
        switch (newGameState)
        {
            case GameState.Idle:
                // Transition from Active to Idle state
                if (currentGameState == GameState.Active)
                {
                    UIManager.Instance.infoPanel.gameObject.SetActive(true);
                    WeatherManager.Instance.ClearWeather();
                    
                }
                else if (currentGameState == GameState.Info)
                {
                    PoolManager.Instance.DeactivateAllPoolObjects();
                    UIManager.Instance.infoPanel.gameObject.SetActive(true);
                    WeatherManager.Instance.ClearWeather();
                   
                }
                break;
            case GameState.Active:
               
                // Transition from Idle to Active state
                if (currentGameState == GameState.Idle)
                {
                    // Perform any necessary actions for transitioning from Idle to Active state
                    UIManager.Instance.idleObjectParent.gameObject.SetActive(false);
                }
                else if (currentGameState == GameState.Info)
                {

                }
                break;
            case GameState.Info:
                if(currentGameState == GameState.Idle)
                {
                   
                }
                else if(currentGameState == GameState.Active) 
                {
                   
                }
                break;
            default:
                break;
        }

        // Update the current game state
        currentGameState = newGameState;
        OnGameStateChanged?.Invoke(currentGameState);
    }

}
