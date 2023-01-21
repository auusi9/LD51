using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Code.Basic
{
    public abstract class TileParent<T> : MonoBehaviour where T : Tile
    {
        [SerializeField] protected T[] _tilesList;
        [SerializeField] private Vector2 _tileSize;
        [SerializeField] private bool _doubleGrid;

        private int _width, _height;
        private int _tiledWidth, _tiledHeight;
        protected T[,] _tiles;

        public int Width => _width;
        public int Height => _height;
        public int TiledWidth => _tiledWidth;
        public int TiledHeight => _tiledHeight;

        private float _xMax;
        private float _xMin;
        private float _yMax;
        private float _yMin;

        private void Awake()
        {
            CalculateTilePositions();
        }

        public void CalculateTilePositions()
        {
            if (_tilesList.Length == 1)
            {
                _xMax = _tileSize.x/2;
                _yMax = _tileSize.y/2;
            }
            else
            {
                _xMax = _tilesList.Max(x => x.RectTransform.anchoredPosition.x);
                _xMin = _tilesList.Min(x => x.RectTransform.anchoredPosition.x);
                _yMax = _tilesList.Max(x => x.RectTransform.anchoredPosition.y);
                _yMin = _tilesList.Min(x => x.RectTransform.anchoredPosition.y);
            }

            _width = Mathf.RoundToInt(Mathf.Abs(_xMax - _xMin) / _tileSize.x) + 1;
            _height = Mathf.RoundToInt(Mathf.RoundToInt(Mathf.Abs(_yMax - _yMin) / _tileSize.y)) + 1;

            _tiledWidth = _width;
            _tiledHeight = _height;
            
            if (_doubleGrid)
            {
                _tiledWidth = _width > 1 ? _width % 2 == 0 ? _width + 1 : _width + ((_width - 1)) : 1;
                _tiledHeight = _height > 1 ? _height % 2 == 0 ? _height + 1 : _height + ((_height - 1)) : 1;   
            }
            
            _tiles = new T[_tiledWidth, _tiledHeight];
            
            foreach (var item in _tilesList)
            {
                RectTransform rectTransform = (RectTransform) item.transform;
                Point position = PositionToGrid(rectTransform.anchoredPosition);
                
                if (_doubleGrid)
                {
                    position.X = (_width - 1) + position.X;
                    position.Y = (_height - 1) + position.Y;
                }
                
                _tiles[position.X, position.Y] = item;
                item.Position = position;
            }
        }

        private Point PositionToGrid(Vector2 position)
        {
            if (_doubleGrid)
            { 
                return new Point((int) (position.x / _tileSize.x), (int) (position.y / _tileSize.y));
            }

            float percentX = (_xMax - _xMin) > 0f ? (position.x - _xMin) / (_xMax - _xMin) : 0f;
            float percentY =  (_yMax - _yMin) > 0f ? (position.y - _yMin) / (_yMax - _yMin) : 0f;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((_width - 1) * percentX);
            int y = Mathf.RoundToInt((_height - 1) * percentY);
            return new Point(x, y);
        }
        
        protected T GetTileAtPosition(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height || _tiles[x, y] == null)
            {
                return null;
            }
            
            return _tiles[x, y];
        }

        public virtual void Rotate()
        {
            foreach (var item in _tilesList)
            {
                RectTransform rectTransform = (RectTransform) item.transform;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.y, -rectTransform.anchoredPosition.x);
            }
            
            CalculateTilePositions();
        }
    }
}