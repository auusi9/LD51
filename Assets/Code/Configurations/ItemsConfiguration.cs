using System;
using Code.Items;
using UnityEngine;

namespace Code.Configurations
{
    [CreateAssetMenu(menuName = "Items/ItemsConfiguration", fileName = "ItemsConfiguration", order = 0)]
    public class ItemsConfiguration : ScriptableObject
    {
        [SerializeField] private Item[] _possibleShapes;
        [SerializeField] private ItemType[] _possibleTypes;

        public int TypeAmount => _possibleTypes.Length;
        public int ShapesAmount => _possibleShapes.Length;

        public Item[] Shapes => _possibleShapes;
        public ItemType[] Types => _possibleTypes;
    }
    
}