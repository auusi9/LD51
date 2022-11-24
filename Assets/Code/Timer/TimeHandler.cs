using System;
using Code.Menus;
using Code.Orders;
using UnityEngine;

namespace Code.Timer
{
    public class TimeHandler : MonoBehaviour
    {
        [SerializeField] private float _totalTime;
        [SerializeField] private EndOfGamePopup _popup;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private GameState _gameState;

        private float _currentTime;
        private bool _hasTriggered = false;

        public event Action TimesUp;

        public float Time => 1 - (_currentTime / _totalTime);

        private void Update()
        {
            if (!_gameState.GameStarted)
            {
                return;
            }
            
            if (_totalTime < _currentTime && !_hasTriggered)
            {
                _hasTriggered = true;
                _popup.OpenPopup(_scoreSystem.TotalScore);
                TimesUp?.Invoke();
            }

            _currentTime += UnityEngine.Time.deltaTime;
        }
    }
}