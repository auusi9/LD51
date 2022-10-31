using UnityEngine;
using UnityEngine.UI;

namespace Shaders
{
    public class ShaderUnscaledTime : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private static readonly int UnscaledTime = Shader.PropertyToID("_UnscaledTime");

        void Update()
        {
            _image.material.SetFloat(UnscaledTime, Time.unscaledTime);
        }

        private void OnDisable()
        {
            _image.material.SetFloat(UnscaledTime, 0f);
        }
    }
}