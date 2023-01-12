using System.Collections;
using UnityEngine;

namespace Code.Basic
{
    public class AudioEffectTransition : SingleInstance<AudioEffectTransition>
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioHighPassFilter _filter;
        [SerializeField] private float _duration;
        [SerializeField] private float _amount;

        public void Resume()
        {
            StopAllCoroutines();
            StartCoroutine(AudioFilterTransitionOut());
        }

        public void Pause()
        {
            StopAllCoroutines();
            StartCoroutine(AudioFilterTransitionIn());
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