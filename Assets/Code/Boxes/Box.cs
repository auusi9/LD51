﻿using System;
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
        [SerializeField] private Image _closedBoxShadow;
        [SerializeField] private Image _boxOpened;
        [SerializeField] private Image _boxOpenedShadow;
        [SerializeField] private Button _closeBoxButton;
        [SerializeField] private Image _closeButtonImage;
        [SerializeField] private Sprite _closeImage;
        [SerializeField] private Sprite _openImage;
        [SerializeField] private AudioSource _boxAudioSource;
        [SerializeField] private AudioClip _openBoxAudio;
        [SerializeField] private AudioClip _closeBoxAudio;
        [SerializeField] private bool _open = true;
        [SerializeField] private Animator _boxAnimator;
        private int _openCloseTrigger = Animator.StringToHash("OpenClose");
        private Vector2 _pitchRange = new Vector2(0.9f, 1.1f);

        public bool IsOpen => _open;

        private void Start()
        {
            if (_open)
            {
                SetOpen(false);
            }
            else
            {
                SetClosed(false);
            }
        }

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
                SetClosed();
            }
            else
            {
                _open = true;
                SetOpen();
            }
        }

        private void SetOpen(bool playSound = true)
        {
            _closeButtonImage.sprite = _closeImage;
            _closedBox.gameObject.SetActive(false);
            _closedBoxShadow.gameObject.SetActive(false);
            _boxOpened.gameObject.SetActive(true);
            _boxOpenedShadow.gameObject.SetActive(true);

            foreach (var tile in _tiles)
            {
                tile.gameObject.SetActive(true);
            }

            _boxAnimator.ResetTrigger(_openCloseTrigger);
            _boxAnimator.SetTrigger(_openCloseTrigger);
            if (playSound)
            {
                _boxAudioSource.pitch = UnityEngine.Random.Range(_pitchRange.x, _pitchRange.y);
                _boxAudioSource.clip = _openBoxAudio;
                _boxAudioSource.Play();
            }
        }

        private void SetClosed(bool playSound = true)
        {
            _closedBox.gameObject.SetActive(true);
            _closedBoxShadow.gameObject.SetActive(true);
            _boxOpened.gameObject.SetActive(false);
            _boxOpenedShadow.gameObject.SetActive(false);
            _closeButtonImage.sprite = _openImage;

            foreach (var tile in _tiles)
            {
                tile.gameObject.SetActive(false);
            }

            _boxAnimator.ResetTrigger(_openCloseTrigger);
            _boxAnimator.SetTrigger(_openCloseTrigger);
            if (playSound)
            {
                _boxAudioSource.pitch = UnityEngine.Random.Range(_pitchRange.x, _pitchRange.y);
                _boxAudioSource.clip = _closeBoxAudio;
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

            boxInfo.ItemCount = items.Count;
            
            return items;
        }

        public class BoxInfo
        {
            public int EmptyTiles;
            public int FillTiles;
            public int ItemTiles;
            public int ItemCount;
        }
    }
}