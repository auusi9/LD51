using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        [SerializeField] private Sprite _tick;
        [SerializeField] private Sprite _cross;
        [SerializeField] private AudioSource _boxAudioSource;
        [SerializeField] private AudioClip _openBoxAudio;
        [SerializeField] private AudioClip _closeBoxAudio;

        private bool _open = true;

        public bool IsOpen => _open;
        
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
                _closeBoxButton.image.sprite = _cross;

                foreach (var tile in _tiles)
                {
                    tile.gameObject.SetActive(false);
                }
                _boxAudioSource.clip = _closeBoxAudio;
                _boxAudioSource.Play();
            }
            else
            {
                _open = true;
                _closeBoxButton.image.sprite = _tick;
                _closedBox.gameObject.SetActive(false);
                _background.gameObject.SetActive(true);
                foreach (var tile in _tiles)
                {
                    tile.gameObject.SetActive(true);
                }
                _boxAudioSource.clip = _openBoxAudio;
                _boxAudioSource.Play();
            }
        }

        public bool Fits(Item item, Vector3 position, List<BoxTile> boxTiles, out BoxTile mainTile)
        {
            mainTile = null;
            ItemTile[,] getTiles = item.GetTiles();

            if (Width < item.Width || Height < item.Height)
            {
                return false;
            }

            float distance = _tilesList.Min(x => Vector3.Distance(position, x.transform.position));
            Point initialPosition =
                _tilesList.FirstOrDefault(x => Vector3.Distance(position, x.transform.position) == distance).Position;
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

        public List<Item> GetItems(out BoxInfo boxInfo)
        {
            boxInfo = new BoxInfo();
            List<Item> items = new List<Item>();
            
            foreach (var tile in _tiles)
            {
                if (tile.Item && !items.Contains(tile.Item) && !tile.Item.IsFill)
                {
                    items.Add(tile.Item);
                    boxInfo.ItemTiles++;
                }
                else if (tile.Item &&!tile.Item.IsFill)
                {
                    boxInfo.ItemTiles++;
                }               
                else if (tile.Item && tile.Item.IsFill)
                {
                    boxInfo.FillTiles++;
                }
                else if(tile.Item == null)
                {
                    boxInfo.EmptyTiles++;
                }
            }

            return items;
        }

        public class BoxInfo
        {
            public int EmptyTiles;
            public int FillTiles;
            public int ItemTiles;
        }
    }
}