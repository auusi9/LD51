using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Services.Leaderboards
{
    public class LeaderBoardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI[] _positions;
        [SerializeField] private TextMeshProUGUI[] _aliases;
        [SerializeField] private TextMeshProUGUI[] _scores;
        [SerializeField] private GameObject _loadingIcon;
        [SerializeField] private GameObject _content;
        [SerializeField] private LeaderboardService _service;
        [SerializeField] private Animator _animator;
        private int _hoverBool = Animator.StringToHash("Hover");

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
            
            for (int i = 0; i < _service.Top.Length; i++)
            {
                _positions[i].text = $"{i + 1}.";
                _aliases[i].text = _service.Top[i].Alias;
                _scores[i].text = _service.Top[i].Score.ToString("N0");
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            _animator.SetBool(_hoverBool, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _animator.SetBool(_hoverBool, false);
        }
    }
}