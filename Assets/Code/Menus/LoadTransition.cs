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
        }

        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (AnimationFinished())
            {
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(WaitForAnimationToFinish());
            }
        }

        private bool AnimationFinished()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_animator.IsInTransition(0);
        }

        private IEnumerator WaitForAnimationToFinish()
        {
            yield return new WaitUntil(AnimationFinished);
            gameObject.SetActive(false);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadSceneAsync(0);
            gameObject.SetActive(true);
        }
    }
}