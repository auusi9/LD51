using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Basic
{
    public class AnimationOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Animator _animator;
        private int _hoverBool = Animator.StringToHash("Hover");

        public void OnPointerEnter(PointerEventData eventData)
        {
            _animator.SetBool(_hoverBool, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _animator.SetBool(_hoverBool, false);
        }
    }
}