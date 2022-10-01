using System;
using System.Drawing;
using System.Linq;
using Code.Basic;
using Code.Boxes;
using UnityEngine;

namespace Code.Items
{
    public class Item : TileParent<ItemTile>
    {
        public Box MyBox;

        public bool IsInBox => MyBox != null;
        public ItemTile[,] GetTiles()
        {
            return _tiles;
        }
    }
}
