using System;
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

        private void Start()
        {
            foreach (Transform child in transform)
            {
                var item = child.GetComponent<MovingItem>();
                if (item != null)
                {
                    item.SetIntoBox(_box);
                }
            }
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