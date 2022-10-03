﻿using System;
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
        private Belt _myBelt;

        private Vector3 _initialPosition;

        private void Start()
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
            base.OnPointerDown(eventData);

            if (_myBelt != null)
            {
                _myBelt.RemoveObjectFromBelt(this);
            }
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
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

        public override void Destroy()
        {
            transform.position = _initialPosition;
            _animator.enabled = true;
            _animator.Rebind();
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
                    _action.DoAction();
                    return true;
                }
            }

            return false;
        }
    }
}