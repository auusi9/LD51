using System.Collections.Generic;
using System.Linq;
using Code.ConveyorBelts;
using UnityEngine;

namespace Code.Services
{
    public class RandomNumberGenerator<T>
    {
        [SerializeField] private List<BeltItemSpawner.RandomItem<T>> _items = new List<BeltItemSpawner.RandomItem<T>>();
        private System.Random _random = new System.Random();
            
        public void Add(double possibility, T item)
        {
            _items.Add(new BeltItemSpawner.RandomItem<T>(item, possibility));
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
                    return item.Item;
                }
            }
            return _items.Last().Item; // Should never happen
        }
    }
}