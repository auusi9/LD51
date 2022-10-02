using System;
using System.Collections.Generic;
using System.Drawing;
using Code.Basic;
using Code.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Boxes
{
    public class Box : TileParent<BoxTile>
    {
        [SerializeField] private Image _closedBox;
        [SerializeField] private Image _background;
        [SerializeField] private Button _closeBoxButton;

        private bool _open = true;
        
        private void OnEnable()
        {
            _closeBoxButton.onClick.AddListener(CloseOpenBox);
        }

        private void OnDisable()
        {
            _closeBoxButton.onClick.RemoveListener(CloseOpenBox);
        }

        private void CloseOpenBox()
        {
            if (_open)
            {
                _open = false;
                _closedBox.gameObject.SetActive(true);
                _background.gameObject.SetActive(false);

                foreach (var tile in _tiles)
                {
                    tile.gameObject.SetActive(false);
                }
            }
            else
            {
                _open = true;
                _closedBox.gameObject.SetActive(false);
                _background.gameObject.SetActive(true);
                foreach (var tile in _tiles)
                {
                    tile.gameObject.SetActive(true);
                }
            }
        }

        public bool Fits(Item item, BoxTile boxTile, List<BoxTile> boxTiles, out BoxTile mainTile)
        {
            mainTile = null;
            ItemTile[,] getTiles = item.GetTiles();

            if (Width < item.Width || Height < item.Height)
            {
                return false;
            }

            Point initialPosition = boxTile.Position;

            for(int i = 0; i < item.TiledWidth; i++)
            {
                for(int j = 0; j < item.TiledHeight; j++)
                {
                    if (getTiles[i, j] != null)
                    {
                        if (FitsWithThisTile(item, getTiles, i, j, initialPosition, boxTiles, out mainTile))
                        {
                            return true;
                        }
                        else
                        {
                            boxTiles.Clear();
                        }
                    }
                }
            }
            
            return false;
        }

        private bool FitsWithThisTile(Item item, ItemTile[,] getTiles, int initialX, int initialY, Point initialPosition, List<BoxTile> boxTiles, out BoxTile mainTile)
        {
            mainTile = null;
            int width = item.Width-1;
            int height = item.Height-1;
            for(int i = 0; i < item.TiledWidth; i++)
            {
                for(int j = 0; j < item.TiledHeight; j++)
                {
                    if (getTiles[i, j] != null)
                    {
                        BoxTile tile = GetTileAtPosition(initialPosition.X + i - initialX, initialPosition.Y + j - initialY);

                        if (i == width && j == height)
                        {
                            mainTile = tile;
                        }
                        
                        if(tile != null && tile.IsAvailable(item))
                        {
                            boxTiles.Add(tile);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void SetItemToBox(Item item, List<BoxTile> tilesToGo)
        {
            foreach (var tile in tilesToGo)
            {
                tile.Item = item;
            }

            item.MyBox = this;
        }

        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            
            foreach (var tile in _tiles)
            {
                if (tile.Item && !items.Contains(tile.Item))
                {
                    items.Add(tile.Item);
                }
            }

            return items;
        }
    }
}