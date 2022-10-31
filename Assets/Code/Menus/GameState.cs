using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Menus
{
    [CreateAssetMenu(menuName = "GameState", fileName = "GameState", order = 0)]
    public class GameState : ScriptableObject
    {
        private bool _gameStarted = false;
        public bool GameStarted => _gameStarted;

        public event Action GameStart;

        private void OnEnable()
        {
            _gameStarted = false;
        }

        public void StartGame()
        {
            _gameStarted = true;
            GameStart?.Invoke();
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
        }

        public void EndGame()
        {
            _gameStarted = false;
        }
    }
}