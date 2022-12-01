using System;
using TMPro;
using UnityEngine;

namespace Code.Services.Leaderboards
{
    public class LeaderBoardUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _positions;
        [SerializeField] private TextMeshProUGUI[] _aliases;
        [SerializeField] private TextMeshProUGUI[] _scores;
        [SerializeField] private LeaderboardService _service;

        private void Start()
        {
            _service.LeaderboardUpdated += LeaderboardUpdated;
        }
        
        private void OnDestroy()
        {
            _service.LeaderboardUpdated -= LeaderboardUpdated;
        }

        private void LeaderboardUpdated()
        {
            for (int i = 0; i < _service.Top.Length; i++)
            {
                _positions[i].text = $"{i + 1}.";
                _aliases[i].text = _service.Top[i].Alias;
                _scores[i].text = _service.Top[i].Score.ToString("N0");
            }
        }
    }
}