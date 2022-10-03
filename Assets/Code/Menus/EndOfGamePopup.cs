using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Menus
{
    public class EndOfGamePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        public void OpenPopup(int score)
        {
            Time.timeScale = 0f;
            gameObject.SetActive(true);
            _textMeshProUGUI.text = $"{score:n0}";
        }

        public void Continue()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}