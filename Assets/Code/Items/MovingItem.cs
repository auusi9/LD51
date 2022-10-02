using System;
using System.Collections.Generic;
using Code.Basic;
using Code.Boxes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Items
{
    public class MovingItem : DraggableObject
    {
        [SerializeField] private Item _item;

        private List<BoxTile> _lastTiles;
        private Transform _defaultParent;
        private Vector3 _oldPosition;
        private bool _isDragging = false;
        
        private void Awake()
        {
            _defaultParent = transform.parent;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right && _isDragging)
            {
                _item.Rotate();
                return;
            }
            
            base.OnPointerDown(eventData);
            _oldPosition = transform.position;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _isDragging = false;
            
            TrySetIntoTile(eventData);
        }

        private void TrySetIntoTile(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            _gridCanvas.GraphicRaycaster.Raycast(eventData, results);

            BoxTile currentTile = null;

            bool invalidPosition = false;

            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<BoxTile>();
                if (slot && slot.Fits(_item, out List<BoxTile> tiles))
                {
                    ClearLastTiles();
                    _lastTiles = tiles;
                    currentTile = slot;
                    slot.SetItemToBox(_item, tiles);
                    transform.SetParent(slot.transform);
                    break;
                }

                if (slot != null)
                    invalidPosition = true;
            }

            if (currentTile == null)
            {
                if (invalidPosition)
                {
                    transform.position = _oldPosition;
                    return;
                }
                
                //Return to old position
                ClearLastTiles();
                
                transform.SetParent(_defaultParent);
                return;
            }
        }

        private void ClearLastTiles()
        {
            if (_lastTiles is {Count: > 0})
            {
                foreach (var lastTile in _lastTiles)
                {
                    lastTile.Item = null;
                }

                _lastTiles.Clear();
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            _isDragging = true;
            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            var results = new List<RaycastResult>();
            _gridCanvas.GraphicRaycaster.Raycast(eventData, results);
            
            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<BoxTile>();
                if (slot && slot.Fits(_item, out List<BoxTile> tiles))
                {
                    transform.position = slot.transform.position;
                    break;
                }
            }
        }
    }
}