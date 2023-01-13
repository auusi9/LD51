using System;
using System.Collections.Generic;
using Code.Basic;
using Code.ConveyorBelts;
using Code.Senders;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Boxes
{
    public class MainMenuBox: DraggableObject
    {
        [SerializeField] private GridCanvas _conveyorBeltCanvas;
        [SerializeField] private MainMenuBoxAction _action;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _boxAudioSource;
        [SerializeField] private AudioClip _boxPickUp;
        [SerializeField] private AudioClip _boxPutDown;
        [SerializeField] private Animator _boxAnimator;
        [SerializeField] private BeltLocator _beltLocator;
        private int _dropTrigger = Animator.StringToHash("Drop");
        private int _grabTrigger = Animator.StringToHash("Grab");
        private Vector2 _pitchRange = new Vector2(0.9f, 1.1f);
        private Belt _myBelt;

        private Vector3 _initialPosition;

        private void Awake()
        {
            _initialPosition = transform.position;
        }

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

            _boxAudioSource.pitch = UnityEngine.Random.Range(_pitchRange.x, _pitchRange.y);
            _boxAudioSource.clip = _boxPickUp;
            _boxAudioSource.Play();

            _boxAnimator.ResetTrigger(_grabTrigger);
            _boxAnimator.SetTrigger(_grabTrigger);
            if (_myBelt != null)
            {
                _myBelt.RemoveObjectFromBelt(this);
                _myBelt = null;
            }
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            if(eventData.pointerId > 0)
                return;
            
            base.OnPointerUp(eventData);
            if (TrySend(eventData))
            {
                return;
            }

            _boxAudioSource.pitch = UnityEngine.Random.Range(_pitchRange.x, _pitchRange.y);
            _boxAudioSource.clip = _boxPutDown;
            _boxAudioSource.Play();

            _boxAnimator.ResetTrigger(_dropTrigger);
            _boxAnimator.SetTrigger(_dropTrigger);
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

        public override void Destroy()
        {
            if (_myBelt == null)
            {
                Belt belt = _beltLocator.GetRandomBelt();
                transform.position = belt.GetInitPosition().position;
                SetBelt(belt);
            }
            else
            {
                Belt belt = _beltLocator.GetOtherBelt(_myBelt);
                transform.position = belt.GetInitPosition().position;
                SetBelt(belt);
            }
        }
        
        private bool TrySend(PointerEventData eventData)
        {
            if (_action == null)
                return false;
            
            var results = new List<RaycastResult>();
            _sender.GraphicRaycaster.Raycast(eventData, results);

            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<Sender>();
                if (slot != null)
                {
                    _action.DoAction();
                    Destroy();
                    return true;
                }
            }

            return false;
        }
    }
}