using System;
using System.Collections.Generic;
using System.Linq;
using Code.Basic;
using Code.Configurations;
using Code.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.ConveyorBelts
{
    public class BeltItemSpawner : MonoBehaviour
    {
        [SerializeField] private ItemsConfiguration _itemsConfiguration;
        [SerializeField] private ComponentPool<MovingItem>[] _poolArray;
        [SerializeField] private Transform _itemsParent;

        private RandomNumberGenerator<int> _randomNumberGenerator;

        private void Start()
        {
            _randomNumberGenerator = new RandomNumberGenerator<int>();

            for (int i = 0; i < _itemsConfiguration.ShapesAmount; i++)
            {
                _randomNumberGenerator.Add(1f, i);
            }
            
            _poolArray = new ComponentPool<MovingItem>[_itemsConfiguration.ShapesAmount];
            for (var i = 0; i < _itemsConfiguration.ShapesAmount; i++)
            {
                var prefab = _itemsConfiguration.Shapes[i];
                _poolArray[i] = new ComponentPool<MovingItem>(10, prefab.GetComponent<MovingItem>(), _itemsParent);
            }
        }

        public MovingItem GetRandomItem()
        {
            int randomInt = _randomNumberGenerator.NextItem();
            MovingItem movingItem = _poolArray[randomInt].GetComponent();
            Debug.Log("New item " + randomInt + " name "+ movingItem.name);
            movingItem.SetPool(_poolArray[randomInt]);
            return movingItem;
        }
        
        [Serializable]
        public class RandomNumberGenerator<T>
        {
            [SerializeField] private List<RandomItem<T>> _items = new List<RandomItem<T>>();
            private System.Random _random = new System.Random();
            
            public void Add(double possibility, T item)
            {
                _items.Add(new RandomItem<T>(item, possibility));
            }
            
            public T NextItem()
            {
                var rand = _random.NextDouble() * _items.Sum(x => x.Possibility);
                double value = 0;
                foreach (var item in _items)
                {
                    value += item.Possibility;
                    if (rand <= value)
                    {
                        item.Possibility = 0;

                        foreach (var it in _items)
                        {
                            if (it != item)
                            {
                                if (it.Possibility <= 1)
                                {
                                    it.Possibility += 0.1f;
                                }
                                else
                                {
                                    it.Possibility += 1f;
                                }
                            }
                        }
                        return item.Item;
                    }
                }
                return _items.Last().Item; // Should never happen
            }
        }

        [Serializable]
        public class RandomItem<T>
        {
            public T Item;
            public double Possibility;

            public RandomItem(T item, double possibility)
            {
                Item = item;
                Possibility = possibility;
            }
        }
    }
}