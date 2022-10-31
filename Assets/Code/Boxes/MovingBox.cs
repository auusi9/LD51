using System.Collections.Generic;
using Code.Basic;
using Code.ConveyorBelts;
using Code.Senders;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Boxes
{
    public class MovingBox : DraggableObject
    {
        [SerializeField] private GridCanvas _conveyorBeltCanvas;
        [SerializeField] private AudioSource _boxAudioSource;
        [SerializeField] private AudioClip _boxPickUp;
        [SerializeField] private AudioClip _boxPutDown;
        [SerializeField] private Animator _boxAnimator;
        private int _dropTrigger = Animator.StringToHash("Drop");
        private int _grabTrigger = Animator.StringToHash("Grab");
        public Box Box;
        private Belt _myBelt;

        public override void SetBelt(Belt belt)
        {
            _myBelt = belt;
            _myBelt.AddObjectToBelt(this);
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.pointerId > 0)
                return;
            
            base.OnPointerDown(eventData);
            _boxAudioSource.clip = _boxPickUp;
            _boxAudioSource.Play();
            _boxAnimator.ResetTrigger(_grabTrigger);
            _boxAnimator.SetTrigger(_grabTrigger);
            if (_myBelt != null)
            {
                _myBelt.RemoveObjectFromBelt(this);
            }
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            if(eventData.pointerId > 0)
                return;
            
            base.OnPointerUp(eventData);
            _boxAudioSource.clip = _boxPutDown;
            _boxAudioSource.Play();
            _boxAnimator.ResetTrigger(_dropTrigger);
            _boxAnimator.SetTrigger(_dropTrigger);
            if (TrySend(eventData))
            {
                return;
            }
            
            TrySetIntoBelt(eventData);
        }
        
        private void TrySetIntoBelt(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            _conveyorBeltCanvas.GraphicRaycaster.Raycast(eventData, results);

            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<Belt>();
                if (slot != null)
                {
                    SetBelt(slot);
                    break;
                }
            }
        }
        
        private bool TrySend(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            _sender.GraphicRaycaster.Raycast(eventData, results);

            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<Sender>();
                if (slot != null)
                {
                    slot.Send(this);
                    return true;
                }
            }

            return false;
        }
    }
}