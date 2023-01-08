using System;
using System.Collections.Generic;
using Code.Basic;
using Code.Boxes;
using Code.ConveyorBelts;
using Code.Menus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Items
{
    public class MovingItem : DraggableObject
    {
        [SerializeField] private Item _item;
        [SerializeField] private GridCanvas _conveyorBeltCanvas;
        [SerializeField] private AudioSource _itemAudioSource;
        [SerializeField] private AudioClip _itemPickUp;
        [SerializeField] private AudioClip _itemPutDown;
        [SerializeField] private BeltLocator _beltLocator;
        [SerializeField] private GameState _gameState;

        private List<BoxTile> _lastTiles;
        private Transform _lastParent;
        private Vector3 _oldPosition;
        private bool _isDragging = false;
        private Belt _myBelt;
        private ComponentPool<MovingItem> _myPool;


        public override bool HasPool => _myPool != null;

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
            if (_gameState.GameStarted)
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
            else
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
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.pointerId > 0)
                return;
            
            if (eventData.button == PointerEventData.InputButton.Right && _isDragging)
            {
                _item.Rotate();
                base.OnDrag(eventData);
                return;
            }

            _oldPosition = transform.position;
            _lastParent = transform.parent;
            transform.SetParent(_gridCanvas.Parent);
            base.OnPointerDown(eventData);
            _isDragging = true;
            _itemAudioSource.clip = _itemPickUp;
            _itemAudioSource.Play();
            
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
            _itemAudioSource.clip = _itemPutDown;
            _itemAudioSource.Play();

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
                var slot = hit.gameObject.GetComponent<Box>();
                List<BoxTile> tiles = new List<BoxTile>();
                if (slot && slot.IsOpen && slot.Fits(_item, eventData.pointerCurrentRaycast.worldPosition, tiles, out BoxTile mainTile))
                {
                    if(mainTile == null)
                        continue;
                    
                    currentTile = SetIntoBox(tiles, mainTile);
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
                    transform.SetParent(_lastParent);
                    return;
                }
                
                //Return to old position
                ClearLastTiles();
                
                transform.SetParent(_gridCanvas.Parent);
                _item.MyBox = null;
            }
        }

        public void SetIntoBox(Box box)
        {
            List<BoxTile> tiles = new List<BoxTile>();

            if (box.Fits(_item, _item.transform.position, tiles, out BoxTile mainTile))
            {
                if(mainTile == null)
                    return;
                    
                SetIntoBox(tiles, mainTile);
            }
        }
        
        private BoxTile SetIntoBox(List<BoxTile> tiles, BoxTile mainTile)
        {
            BoxTile currentTile;
            ClearLastTiles();
            _lastTiles = tiles;
            currentTile = mainTile;
            mainTile.SetItemToBox(_item, tiles);
            transform.SetParent(mainTile.transform);
            return currentTile;
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
            if(eventData.pointerId > 0)
                return;
            
            base.OnDrag(eventData);
            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            var results = new List<RaycastResult>();
            _gridCanvas.GraphicRaycaster.Raycast(eventData, results);
            
            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<Box>();
                List<BoxTile> tiles = new List<BoxTile>();
                if (slot && slot.IsOpen && slot.Fits(_item, eventData.pointerCurrentRaycast.worldPosition, tiles, out BoxTile mainTile))
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