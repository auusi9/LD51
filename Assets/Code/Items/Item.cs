using System;
using System.Drawing;
using System.Linq;
using Code.Basic;
using Code.Boxes;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Items
{
    public class Item : TileParent<ItemTile>
    {
        [HideInInspector] public Box MyBox;
        [SerializeField] public Image _image;

        public ItemTile[,] GetTiles()
        {
            return _tiles;
        }
    }
}
