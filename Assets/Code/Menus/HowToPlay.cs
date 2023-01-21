using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Menus
{
    public class HowToPlay : MonoBehaviour
    {
        [SerializeField] private CanvasGroup[] _pages;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _leftButton;
        [SerializeField] private float _pageTransitionTime;
        [SerializeField] private Animator _animator;

        private int _currentPage;
        private int _outTrigger = Animator.StringToHash("Out");

        private void OnEnable()
        {
            _currentPage = 0;
            SetPage();
        }

        public void DisableObject()
        {
            _animator.SetTrigger(_outTrigger);
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

        public void NextPage()
        {
            _currentPage++;

            if (_currentPage > _pages.Length - 1)
            {
                return;
            }
            SetPage();
        }

        public void PreviousPage()
        {
            _currentPage--;
            if (_currentPage < 0)
            {
                return;
            }
            SetPage();
        }

        private void SetPage()
        {
            for (int i = 0; i < _pages.Length; i++)
            {
                if (i == _currentPage)
                {
                    StopAllCoroutines();
                    StartCoroutine(EnablePage(_pages[i]));
                }
                else
                {
                    _pages[i].alpha = 0f;
                }
            }

            if (_currentPage == _pages.Length - 1)
            {
                _rightButton.interactable = false;
                _leftButton.interactable = true;
            }
            else if(_currentPage == 0)
            {
                _rightButton.interactable = true;
                _leftButton.interactable = false;
            }
            else
            {
                _rightButton.interactable = true;
                _leftButton.interactable = true;
            }
        }

        private IEnumerator EnablePage(CanvasGroup page)
        {
            page.gameObject.SetActive(true);
            page.alpha = 0f;
            float time = 0f;
            while (_pageTransitionTime > time)
            {
                float t = time/_pageTransitionTime;
                t = t * t * (3f - 2f * t);
                page.alpha = Mathf.Lerp(page.alpha, 1f, t);
                yield return 0;
                time += Time.deltaTime;
            }

            page.alpha = 1f;
        }
    }
}