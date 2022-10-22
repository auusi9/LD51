using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Code.Basic;
using Code.Items;
using Code.Menus;
using UnityEngine;

namespace Code.ConveyorBelts
{
    public class Belt : MonoBehaviour
    {
        [SerializeField] private float _timeBetweenItems = 10f;
        [SerializeField] private Transform _initialPosition;
        [SerializeField] private Transform _finalPosition;
        [SerializeField] private float _velocity;
        [SerializeField] private BeltItemSpawner _beltItemSpawner;
        [SerializeField] private GameState _gameState;
        
        private List<DraggableObject> _objectsInBelt = new List<DraggableObject>();
        private float _timeWithoutItem = 0f;
        private Vector3 _beltDirection;

        private void Start()
        {
            _beltDirection = _finalPosition.position - _initialPosition.position;
        }

        private void Update()
        {
            if (_timeBetweenItems <= _timeWithoutItem && _gameState.GameStarted)
            {
                _timeWithoutItem = 0;
                DraggableObject item = _beltItemSpawner.GetRandomItem();
                item.SetBelt(this);
                item.transform.position = _initialPosition.position;
                item.gameObject.SetActive(true);
            }
            else
            {
                _timeWithoutItem += Time.deltaTime;
            }

            List<DraggableObject> toDestroy = new List<DraggableObject>();
            foreach (var obj in _objectsInBelt)
            {
                obj.transform.Translate(_beltDirection.normalized * Time.deltaTime * _velocity);

                if (Vector3.Distance(obj.transform.position, _finalPosition.position) < 1f)
                {
                    toDestroy.Add(obj);
                }
            }

            foreach (var obj in toDestroy)
            {
                RemoveObjectFromBelt(obj);
                obj.Destroy();
            }
        }

        public void AddObjectToBelt(DraggableObject obj)
        {
            if(_objectsInBelt.Contains(obj))
               return;
               
            _objectsInBelt.Add(obj);
        }

        public void RemoveObjectFromBelt(DraggableObject obj)
        {
            if(!_objectsInBelt.Contains(obj))
                return;
               
            _objectsInBelt.Remove(obj);
        }
    }
}