using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameState _gameState;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _fxSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Animator _pauseAnimator;
        private int _outTrigger = Animator.StringToHash("Out");

        public void Show()
        {
            _gameState.PauseGame();
            gameObject.SetActive(true);

            if (_audioMixer.GetFloat("FXVolume", out float fxValue))
            {
                _fxSlider.SetValueWithoutNotify(fxValue);
            }
            
            if (_audioMixer.GetFloat("MusicVolume", out float musicValue))
            {
                _musicSlider.SetValueWithoutNotify(musicValue);
            }
        }

        public void Resume()
        {
            _pauseAnimator.SetTrigger(_outTrigger);
        }

        public void MainMenu()
        {
            _gameState.ResumeGame();
            _gameState.EndGame();
            LoadTransition.Instance.LoadMainMenu();
        }

        private void Hide()
        {
            _gameState.ResumeGame();
            gameObject.SetActive(false);
        }

        public void SetFXVolume(float fxVolume)
        {
            _audioMixer.SetFloat("FXVolume", fxVolume);
        }

        public void SetMusicVolume(float musicVolume)
        {
            _audioMixer.SetFloat("MusicVolume", musicVolume);
        }
    }
}