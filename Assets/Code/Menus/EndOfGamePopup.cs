using System;
using Code.Basic;
using Code.Services.Leaderboards;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Menus
{
    public class EndOfGamePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private TMP_InputField _tmpInputField;
        [SerializeField] private GameState _gameState;
        [SerializeField] private Button _submitButton;
        [SerializeField] private Button _mainMenu;
        [SerializeField] private Button _continue;
        [SerializeField] private LeaderboardService _leaderboardService;
        [SerializeField] private LeaderboardEndPopup _leaderboardEndPopup;
        [SerializeField] private Animator _disableSubmit;

        private int _disableTrigger = Animator.StringToHash("Disable");
        private int _score;
        
        private void Start()
        {
            _tmpInputField.onValueChanged.AddListener(NameSet);
        }

        private void NameSet(string newAlias)
        {
            if (string.IsNullOrEmpty(newAlias) || newAlias.Length < 3)
            {
                _submitButton.interactable = false;
            }
            else
            {
                _leaderboardService.SetAlias(newAlias);
                _submitButton.interactable = true;
            }
        }

        public void Submit()
        {
            _leaderboardService.NewEntry(_score);
            _submitButton.interactable = false;
            _disableSubmit.SetTrigger(_disableTrigger);
            _tmpInputField.interactable = false;
            _leaderboardEndPopup.DisableNewEntry();
        }
        
        public void OpenPopup(int score)
        {
            AudioEffectTransition.Get().Pause();
            _score = score;
            _gameState.PauseGame();
            gameObject.SetActive(true);
            _textMeshProUGUI.text = $"{score:n0}";
            _tmpInputField.SetTextWithoutNotify(_leaderboardService.GetAlias());
            NameSet(_tmpInputField.text);
            _tmpInputField.gameObject.SetActive(true);
            _mainMenu.gameObject.SetActive(true);
            _continue.gameObject.SetActive(true);
            _submitButton.gameObject.SetActive(true);
            _tmpInputField.interactable = true;
            _leaderboardEndPopup.SetScore(score);
        }

        public void Continue()
        {
            AudioEffectTransition.Get().Resume();
            _gameState.ResumeGame();
            gameObject.SetActive(false);
        }

        public void MainMenu()
        {
            AudioEffectTransition.Get().Resume();
            _gameState.ResumeGame();
            _gameState.EndGame();
            LoadTransition.Instance.LoadMainMenu();
        }
    }
}