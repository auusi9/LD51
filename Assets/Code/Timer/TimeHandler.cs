using System;
using UnityEngine;

namespace Code.Timer
{
    public class TimeHandler : MonoBehaviour
    {
        [SerializeField] private float _totalTime;

        private float _currentTime;
        private bool _hasTriggered = false;

        public event Action TimesUp;

        public float Time => _currentTime / _totalTime;

        private void Update()
        {
            if (_totalTime < _currentTime && !_hasTriggered)
            {
                _hasTriggered = true;
                TimesUp?.Invoke();
            }

            _currentTime += UnityEngine.Time.deltaTime;
        }
    }
}