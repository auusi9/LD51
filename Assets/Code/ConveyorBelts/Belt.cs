using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.ConveyorBelts
{
    public class Belt : MonoBehaviour
    {
        [SerializeField] private float _timeBetweenItems = 10f;
        [SerializeField] private Transform _initialPosition;
        [SerializeField] private Transform _finalPosition;
        [SerializeField] private float _velocity;
        
        private List<Transform> _objectsInBelt = new List<Transform>();
        private float _timeWithoutItem = 0f;
        private Vector3 _beltDirection;

        private void Start()
        {
            _beltDirection = _finalPosition.position - _initialPosition.position;
        }

        private void Update()
        {
            if (_timeBetweenItems <= _timeWithoutItem)
            {
                //NewItem
                _timeWithoutItem = 0;
            }
            else
            {
                _timeWithoutItem += Time.deltaTime;
            }

            List<Transform> toDestroy = new List<Transform>();
            foreach (var obj in _objectsInBelt)
            {
                obj.Translate(_beltDirection.normalized * Time.deltaTime * _velocity);

                if (Vector3.Distance(obj.position, _finalPosition.position) < 1f)
                {
                    toDestroy.Add(obj);
                }
            }

            foreach (var obj in toDestroy)
            {
                RemoveObjectFromBelt(obj);
                Destroy(obj.gameObject);
            }
        }

        public void AddObjectToBelt(Transform obj)
        {
            if(_objectsInBelt.Contains(obj))
               return;
               
            _objectsInBelt.Add(obj);
        }

        public void RemoveObjectFromBelt(Transform obj)
        {
            if(!_objectsInBelt.Contains(obj))
                return;
               
            _objectsInBelt.Remove(obj);
        }
    }
}