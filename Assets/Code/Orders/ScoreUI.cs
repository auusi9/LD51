using System;
using TMPro;
using UnityEngine;

namespace Code.Orders
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private void Start()
        {
            _scoreSystem.ScoreUpdated += ScoreUpdated;
            _textMeshProUGUI.text = "0";
        }

        private void OnDestroy()
        {
            _scoreSystem.ScoreUpdated -= ScoreUpdated;
        }
        
        private void ScoreUpdated(int obj)
        {
            _textMeshProUGUI.text = $"{obj:n0}";
        }
    }
}