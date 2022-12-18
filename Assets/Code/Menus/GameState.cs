using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Menus
{
    [CreateAssetMenu(menuName = "GameState", fileName = "GameState", order = 0)]
    public class GameState : ScriptableObject
    {
        private bool _gameStarted = false;
        private bool _gamePaused = false;
        public bool GameStarted => _gameStarted;
        public bool GamePaused => _gamePaused;

        public event Action GameStart;

        private void OnEnable()
        {
            _gameStarted = false;
        }

        public void StartGame()
        {
            _gameStarted = true;
            _gamePaused = false;
            GameStart?.Invoke();
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            _gamePaused = true;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            _gamePaused = false;
        }

        public void EndGame()
        {
            _gameStarted = false;
        }
    }
}