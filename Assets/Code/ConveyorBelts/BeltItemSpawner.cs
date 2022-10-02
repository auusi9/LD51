using System;
using Code.Basic;
using Code.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.ConveyorBelts
{
    public class BeltItemSpawner : MonoBehaviour
    {
        [SerializeField] private MovingItem[] _itemsPrefab;
        [SerializeField] private ComponentPool<MovingItem>[] _poolArray;
        [SerializeField] private Transform _itemsParent;

        private void Start()
        {
            _poolArray = new ComponentPool<MovingItem>[_itemsPrefab.Length];
            for (var i = 0; i < _itemsPrefab.Length; i++)
            {
                var prefab = _itemsPrefab[i];
                _poolArray[i] = new ComponentPool<MovingItem>(10, _itemsPrefab[i], _itemsParent);
            }
        }

        public MovingItem GetRandomItem()
        {
            int randomInt = Random.Range(0, _poolArray.Length);
            MovingItem movingItem = _poolArray[randomInt].GetComponent();
            movingItem.SetPool(_poolArray[randomInt]);
            return _poolArray[Random.Range(0, _poolArray.Length)].GetComponent();
        }
    }
}