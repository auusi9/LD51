using System.Collections;
using UnityEngine;

namespace Code.Basic
{
    public class AudioEffectTransition : SingleInstance<AudioEffectTransition>
    {
        [SerializeField] private AudioSource _source;
        //[SerializeField] private AudioHighPassFilter _filter;
        [SerializeField] private AudioLowPassFilter _filter;
        [SerializeField] private float _duration;
        [SerializeField] private float _startAmount;
        [SerializeField] private float _endAmount;

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
            float amountPerSec = (_endAmount - _filter.cutoffFrequency) / _duration;

            for (float t = 0f; t<= _duration; t += Time.unscaledDeltaTime)
            {
                _filter.cutoffFrequency += amountPerSec * Time.unscaledDeltaTime;
                yield return null;
            }

            _filter.cutoffFrequency = _endAmount;
        }

        private IEnumerator AudioFilterTransitionOut()
        {
            float amountPerSec = (_startAmount - _filter.cutoffFrequency) / _duration;

            for (float t = 0f; t <= _duration; t += Time.unscaledDeltaTime)
            {
                _filter.cutoffFrequency += amountPerSec * Time.unscaledDeltaTime;
                yield return null;
            }

            _filter.cutoffFrequency = _startAmount;
        }
    }
}