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
        [HideInInspector] public Box MyBox;

        public ItemTile[,] GetTiles()
        {
            return _tiles;
        }
    }
}
