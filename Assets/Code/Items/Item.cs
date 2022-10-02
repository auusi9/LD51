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
        [SerializeField] public Image _image;
        [SerializeField] private ItemType _itemType;

        public ItemType ItemType => _itemType;
        
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
        }
    }
}
