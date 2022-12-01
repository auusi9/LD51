using System;
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
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _mainMenu;
        [SerializeField] private Button _continue;
        [SerializeField] private LeaderboardService _leaderboardService;

        private int _score;
        
        private void Start()
        {
            _tmpInputField.onEndEdit.AddListener(NameSet);
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
            Cancel();
        }

        public void Cancel()
        {
            _mainMenu.gameObject.SetActive(true);
            _continue.gameObject.SetActive(true);
            _submitButton.gameObject.SetActive(false);
            _cancelButton.gameObject.SetActive(false);
            _tmpInputField.gameObject.SetActive(false);
        }

        public void OpenPopup(int score)
        {
            _score = score;
            _gameState.PauseGame();
            gameObject.SetActive(true);
            _textMeshProUGUI.text = $"{score:n0}";
            _tmpInputField.SetTextWithoutNotify(_leaderboardService.GetAlias());
            NameSet(_tmpInputField.text);
            _tmpInputField.gameObject.SetActive(true);
            _mainMenu.gameObject.SetActive(false);
            _continue.gameObject.SetActive(false);
            _submitButton.gameObject.SetActive(true);
            _cancelButton.gameObject.SetActive(true);
        }

        public void Continue()
        {
            _gameState.ResumeGame();
            gameObject.SetActive(false);
        }

        public void MainMenu()
        {
            _gameState.ResumeGame();
            _gameState.EndGame();
            SceneManager.LoadScene(0);
        }
    }
}