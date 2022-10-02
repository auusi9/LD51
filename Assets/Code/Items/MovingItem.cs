using System;
using System.Collections.Generic;
using Code.Basic;
using Code.Boxes;
using Code.ConveyorBelts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Items
{
    public class MovingItem : DraggableObject
    {
        [SerializeField] private Item _item;
        [SerializeField] private GridCanvas _conveyorBeltCanvas;

        private List<BoxTile> _lastTiles;
        private Transform _defaultParent;
        private Vector3 _oldPosition;
        private bool _isDragging = false;
        private Belt _myBelt;
        private ComponentPool<MovingItem> _myPool;


        public override bool HasPool => _myPool != null;

        private void Awake()
        {
            _defaultParent = transform.parent;
        }

        public void SetPool(ComponentPool<MovingItem> myPool)
        {
            _myPool = myPool;
        }

        protected override void ReturnToPool()
        {
            _myPool.ReturnMono(this);
        }

        public override void SetBelt(Belt belt)
        {
            _myBelt = belt;
            _myBelt.AddObjectToBelt(this);
        }
        
        public override void Destroy()
        {
            if (HasPool)
            {
                ReturnToPool();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right && _isDragging)
            {
                _item.Rotate();
                base.OnDrag(eventData);
                return;
            }
            
            base.OnPointerDown(eventData);
            _isDragging = true;
            _oldPosition = transform.position;

            if (_myBelt != null)
            {
                _myBelt.RemoveObjectFromBelt(this);
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            base.OnPointerUp(eventData);
            _isDragging = false;
            
            TrySetIntoTile(eventData);

            if (!_item.MyBox)
            {
                TrySetIntoBelt(eventData);
            }
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

        private void TrySetIntoTile(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            _gridCanvas.GraphicRaycaster.Raycast(eventData, results);

            BoxTile currentTile = null;

            bool invalidPosition = false;

            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<BoxTile>();
                List<BoxTile> tiles = new List<BoxTile>();
                if (slot && slot.Fits(_item,  tiles, out BoxTile mainTile))
                {
                    if(mainTile == null)
                        continue;
                    
                    ClearLastTiles();
                    _lastTiles = tiles;
                    currentTile = mainTile;
                    mainTile.SetItemToBox(_item, tiles);
                    transform.SetParent(mainTile.transform);
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
                _item.MyBox = null;
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
            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            var results = new List<RaycastResult>();
            _gridCanvas.GraphicRaycaster.Raycast(eventData, results);
            
            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<BoxTile>();
                List<BoxTile> tiles = new List<BoxTile>();
                if (slot && slot.Fits(_item, tiles, out BoxTile mainTile))
                {
                    if (mainTile != null)
                    {
                        transform.position = mainTile.transform.position;
                    }
                    break;
                }
            }
        }
    }
}