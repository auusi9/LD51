using System;
using System.Linq;
using Code.Items;
using UnityEngine;

namespace Code.Configurations
{
    [CreateAssetMenu(menuName = "Items/ItemsConfiguration", fileName = "ItemsConfiguration", order = 0)]
    public class ItemsConfiguration : ScriptableObject
    {
        [SerializeField] private ItemConfig[] _itemConfigs;
        [SerializeField] private ItemType[] _possibleTypes;

        public int TypeAmount => _possibleTypes.Length;
        public int ShapesAmount => _itemConfigs.Length;

        public Item[] Shapes => _itemConfigs.Select(x => x.Item).ToArray();
        public ItemType[] Types => _possibleTypes;
        public float[] Chances => _itemConfigs.Select(x => x.Chance).ToArray();
    }
    
    [Serializable]
    public class ItemConfig
    {
        public Item Item;
        public float Chance = 1f;
    }
}