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
        private Belt _myBelt;
        
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

            Belt currentBelt = null;

            bool invalidPosition = false;

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

            Sender currentBelt = null;

            bool invalidPosition = false;

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