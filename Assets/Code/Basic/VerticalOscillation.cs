using UnityEngine;

namespace Code.Basic
{
    public class VerticalOscillation : MonoBehaviour
    {
        [SerializeField] float _scale = 1f;
        [SerializeField] float _speed = 1f;
        [SerializeField] float _timeOffset = 0f;

        private RectTransform _rectTransform;
        private Vector2 _localPos;
        private Vector2 _startingLocalPos;
        private float _timer = 0;

        void OnEnable()
        {
            _rectTransform = transform as RectTransform;
            _localPos = _rectTransform.localPosition;
            _startingLocalPos = _rectTransform.localPosition;
            _timer = _timeOffset;
        }

        void Update()
        {
            _timer += Time.unscaledDeltaTime;
            float cycle = _timer * _speed;
            
            const float tau = Mathf.PI * 2f;
            float rawCosWave = Mathf.Cos(cycle * tau);

            float offset = rawCosWave * _scale;
            _localPos.y = _startingLocalPos.y + offset;
            _rectTransform.localPosition = _localPos;
        }
    }
}