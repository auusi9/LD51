using System.Collections.Generic;
using Code.Basic;
using Code.Items;
using UnityEngine;

namespace Code.Boxes
{
    public class BoxTile : Tile
    {
        [SerializeField] private Box _box;

        [HideInInspector] public Item Item;
        
        public bool Fits(Item item, List<BoxTile> result, out BoxTile mainTile)
        {
            mainTile = null;
            if (!IsAvailable(item))
            {
                return false;
            }
            
            return _box.Fits(item, this, result, out mainTile);
        }

        public void SetItemToBox(Item item, List<BoxTile> tilesToGo)
        {
            _box.SetItemToBox(item, tilesToGo);
        }

        public bool IsAvailable(Item item)
        {
            if (Item != null && Item != item)
            {
                return false;
            }

            return true;
        }
    }
}