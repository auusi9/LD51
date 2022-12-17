using System;
using UnityEngine;

namespace Code.Menus
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private GameState _gameState;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _gameState.GameStarted && !_gameState.GamePaused)
            {
                _pauseMenu.Show();
            }
        }

        public void ShowPauseMenu()
        {
            _pauseMenu.Show();
        }
    }
}