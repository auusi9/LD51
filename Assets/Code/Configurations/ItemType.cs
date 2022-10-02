using UnityEngine;

namespace Code.Configurations
{
    [CreateAssetMenu(menuName = "Items/ItemType", fileName = "ItemType", order = 0)]
    public class ItemType : ScriptableObject
    {
        public Material Material;
        public Color Color;
    }
}