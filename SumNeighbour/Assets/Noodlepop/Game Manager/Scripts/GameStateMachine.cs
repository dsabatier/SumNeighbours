using RoboRyanTron.SearchableEnum;
using System;
using UnityEngine;
using Noodlepop.GameEvents;

namespace Noodlepop
{
    /// <summary>
    /// Tracks game state, changes game state and raises game state changed event.
    /// </summary>
    [System.Serializable]
    public class GameStateMachine
    {
        [SerializeField] private GameEventGameState _onGameStateChanged;
        [SerializeField, SearchableEnum] private GameState _currentGameState;
        public GameState GameState => _currentGameState;


        public void UpdateState(GameState newState)
        {
            GameState previousState = _currentGameState;
            _currentGameState = newState;

            switch (_currentGameState)
            {
                case GameState.MainMenu:
                break;
                case GameState.InGame:
                    break;
                case GameState.Loading:
                break;
                default:
                throw new NotImplementedException();
            }

            _onGameStateChanged.Raise(previousState, _currentGameState);
        }
    }

    public enum GameState
    {
        MainMenu,
        InGame,
        Loading
    }
}