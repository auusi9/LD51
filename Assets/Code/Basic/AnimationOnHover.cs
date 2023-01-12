using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Basic
{
    public class AnimationOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _hoverEnter;
        [SerializeField] private AudioSource _hoverExit;
        private int _hoverBool = Animator.StringToHash("Hover");

        public void OnPointerEnter(PointerEventData eventData)
        {
            _animator.SetBool(_hoverBool, true);
           
            if(_hoverEnter != null)
                _hoverEnter.Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _animator.SetBool(_hoverBool, false);
            
            if(_hoverExit != null)
                _hoverExit.Play();
        }
    }
}