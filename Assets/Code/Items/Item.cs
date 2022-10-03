using System;
using System.Drawing;
using System.Linq;
using Code.Basic;
using Code.Boxes;
using Code.Configurations;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Items
{
    public class Item : TileParent<ItemTile>
    {
        [HideInInspector] public Box MyBox;
        [SerializeField] private Image _image;
        [SerializeField] private Image _shadow;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private bool _isFill;
        
        public Sprite Icon;
        
        public ItemType ItemType => _itemType;
        public bool IsFill => _isFill;
        
        public ItemTile[,] GetTiles()
        {
            return _tiles;
        }

        public void SetItemType(ItemType itemType)
        {
            _itemType = itemType;
        }

        public override void Rotate()
        {
            base.Rotate();
            _image.transform.Rotate(Vector3.forward, -90f);
            _shadow.transform.Rotate(Vector3.forward, -90f);
        }
    }
}
