using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Basic
{
    public class AudioEffectTransition : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioHighPassFilter _filter;
        [SerializeField] private float _duration;
        [SerializeField] private float _amount;
        private bool _isPaused = false;

        private void Update()
        {
            if (Time.timeScale == 0f && _isPaused == false)
            {
                _isPaused = true;
                StartCoroutine(AudioFilterTransitionIn());
            }

            if (Time.timeScale != 0f && _isPaused == true)
            {
                _isPaused = false;
                StartCoroutine(AudioFilterTransitionOut());
            }
        }

        private IEnumerator AudioFilterTransitionIn()
        {
            float amountPerSec = _amount / _duration;

            for (float t = 0f; t<= _duration; t += Time.unscaledDeltaTime)
            {
                _filter.cutoffFrequency = amountPerSec * t;
                yield return null;
            }

            _filter.cutoffFrequency = _amount;
        }

        private IEnumerator AudioFilterTransitionOut()
        {
            float amountPerSec = _amount / _duration;

            for (float t = 0f; t <= _duration; t += Time.unscaledDeltaTime)
            {
                _filter.cutoffFrequency = _amount - (amountPerSec * t);
                yield return null;
            }

            _filter.cutoffFrequency = 0f;
        }
    }
}