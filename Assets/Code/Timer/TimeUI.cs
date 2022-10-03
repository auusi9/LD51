using UnityEngine;
using UnityEngine.UI;

namespace Code.Timer
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TimeHandler _timeHandler;
        [SerializeField] private Image _image;

        private void Update()
        {
            _image.fillAmount = _timeHandler.Time;
        }
    }
}