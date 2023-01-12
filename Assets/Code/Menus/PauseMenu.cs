using Code.Basic;
using UnityEngine;
using UnityEngine.Audio;
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
                _fxSlider.SetValueWithoutNotify(Mathf.Pow(10, fxValue/20));
            }
            
            if (_audioMixer.GetFloat("MusicVolume", out float musicValue))
            {
                _musicSlider.SetValueWithoutNotify(Mathf.Pow(10, musicValue / 20));
            }
            
            AudioEffectTransition.Get().Pause();
        }

        public void Resume()
        {
            _pauseAnimator.SetTrigger(_outTrigger);
            AudioEffectTransition.Get().Resume();
        }

        public void MainMenu()
        {
            AudioEffectTransition.Get().Resume();
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
            _audioMixer.SetFloat("FXVolume", Mathf.Log10(fxVolume) * 20);
        }

        public void SetMusicVolume(float musicVolume)
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        }
    }
}