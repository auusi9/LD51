using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.ConveyorBelts
{
    [CreateAssetMenu(menuName = "Belts/BeltLocator", fileName = "BeltLocator", order = 0)]
    public class BeltLocator : ScriptableObject
    {
        private List<Belt> _belts = new List<Belt>();

        public void OnEnable()
        {
            _belts.Clear();
        }

        public void AddBelt(Belt belt)
        {
            if (!_belts.Contains(belt))
            {
                _belts.Add(belt);
            }
        }

        public void RemoveBelt(Belt belt)
        {
            if (_belts.Contains(belt))
            {
                _belts.Remove(belt);
            }
        }

        public Belt GetOtherBelt(Belt belt)
        {
            return _belts.FirstOrDefault(x => x != belt);
        }
        
        public Belt GetRandomBelt()
        {
            return _belts[Random.Range(0, _belts.Count)];
        }
    }
}