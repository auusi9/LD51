using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Menus
{
    public class EndOfGamePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private GameState _gameState;

        public void OpenPopup(int score)
        {
            _gameState.PauseGame();
            gameObject.SetActive(true);
            _textMeshProUGUI.text = $"{score:n0}";
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