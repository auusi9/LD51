using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Menus
{
    public class SceneOpening : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _audioSource2;
        [SerializeField] private AudioSource _audioSource3;
        
        private readonly int _outHash = Animator.StringToHash("Out");
        private readonly int _inHash = Animator.StringToHash("In");

        private static bool _gameStarted;
        
        private void Awake()
        {
            if (_gameStarted)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(StartAnimation());
        }

        private IEnumerator StartAnimation()
        {
            yield return 0;
            yield return 0;
            _animator.SetTrigger(_inHash);
        }

        public void PlayAudioSource()
        {
            _audioSource.Play();
        }
        public void PlayAudioSource2()
        {
            _audioSource2.Play();
        }
        
        public void PlayAudioSource3()
        {
            _audioSource3.Play();
        }

        public void StartGame()
        {
            _gameStarted = true;
            _animator.SetTrigger(_outHash);
            StartCoroutine(WaitForAnimationToFinish());
        }
        
        private bool AnimationFinished()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_animator.IsInTransition(0);
        }
        
        private IEnumerator WaitForAnimationToFinish()
        {
            yield return 0;
            yield return new WaitUntil(AnimationFinished);
            gameObject.SetActive(false);
        }
    }
}