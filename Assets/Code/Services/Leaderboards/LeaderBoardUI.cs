using System;
using TMPro;
using UnityEngine;

namespace Code.Services.Leaderboards
{
    public abstract class LeaderBoardUI : MonoBehaviour
    {
        [SerializeField] protected LeaderboardUIElement[] _uiElements;
        [SerializeField] private GameObject _loadingIcon;
        [SerializeField] private GameObject _content;
        [SerializeField] protected LeaderboardService _service;

        private void Start()
        {
            _service.LeaderboardUpdated += LeaderboardUpdated;
            _loadingIcon.SetActive(true);
            _content.SetActive(false);
        }
        
        private void OnDestroy()
        {
            _service.LeaderboardUpdated -= LeaderboardUpdated;
        }

        private void LeaderboardUpdated()
        {
            _content.SetActive(true);
            _loadingIcon.SetActive(false);

            LeaderboardPosition[] entries = GetEntries();
            
            if(entries == null)
                return;
            
            for (int i = 0; i < entries.Length; i++)
            {
                _uiElements[i].SetElement(entries[i].Position, entries[i].Alias, entries[i].Score);
            }
        }

        protected abstract LeaderboardPosition[] GetEntries();
    }
}