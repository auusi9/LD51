using UnityEngine;
using UnityEngine.UI;

namespace Code.Timer
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TimeHandler _timeHandler;
        [SerializeField] private Image _image;
        [SerializeField] private Color _color01;
        [SerializeField] private Color _color02;
        [SerializeField] private Color _color03;

        private void Update()
        {
            _image.fillAmount = _timeHandler.Time;
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (_timeHandler.Time < 0.2)
            {
                _image.color = _color03;
            }
            else if (_timeHandler.Time < 0.6)
            {
                _image.color = _color02;
            }
            else
            {
                _image.color = _color01;
            }
        }
    }
}