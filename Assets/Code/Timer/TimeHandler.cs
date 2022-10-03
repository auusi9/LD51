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

        private float _currentTime;
        private bool _hasTriggered = false;

        public event Action TimesUp;

        public float Time => _currentTime / _totalTime;

        private void Update()
        {
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