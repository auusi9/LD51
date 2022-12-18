using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Menus
{
    public class LoadTransition : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        public static LoadTransition Instance;

        private int _outHash = Animator.StringToHash("Out");

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
            
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
            gameObject.SetActive(false);
        }

        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (AnimationFinished())
            {
                _animator.SetTrigger(_outHash);
                StartCoroutine(WaitForAnimationToFinish());
            }
        }

        private bool AnimationFinished()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_animator.IsInTransition(0);
        }

        private IEnumerator WaitForOut()
        {
            yield return new WaitUntil(AnimationFinished);
            SceneManager.LoadSceneAsync(0);
        }

        private IEnumerator WaitForAnimationToFinish()
        {
            yield return 0;
            yield return new WaitUntil(AnimationFinished);
            gameObject.SetActive(false);
        }

        public void LoadMainMenu()
        {
            gameObject.SetActive(true);
            StartCoroutine(WaitForOut());
        }
    }
}