using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Code.Basic
{
    public abstract class TileParent<T> : MonoBehaviour where T : Tile
    {
        [SerializeField] private T[] _tilesList;
        [SerializeField] private Vector2 _tileSize;

        private int _width, _height;
        protected T[,] _tiles;

        public int Width => _width;
        public int Height => _height;
        
        private float _xMax;
        private float _xMin;
        private float _yMax;
        private float _yMin;

        private void Start()
        {
            CalculateTilePositions();
        }

        private Point PositionToGrid(Vector2 position, Vector2 pivot)
        {
            float percentX = (position.x - _xMin) / (_xMax - _xMin);
            float percentY = (position.y - _yMin) / (_yMax - _yMin);
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

        public void CalculateTilePositions()
        {
            _xMax = _tilesList.Max(x => x.RectTransform.anchoredPosition.x);
            _xMin = _tilesList.Min(x => x.RectTransform.anchoredPosition.x);
            _yMax = _tilesList.Max(x => x.RectTransform.anchoredPosition.y);
            _yMin = _tilesList.Min(x => x.RectTransform.anchoredPosition.y);

            if (_xMax == 0)
            {
                _xMax = _tileSize.x;
            }
            
            if (_yMax == 0)
            {
                _yMax = _tileSize.y;
            }

            _width = Mathf.RoundToInt(Mathf.Abs(_xMax - _xMin) / _tileSize.x) + 1;
            _height = Mathf.RoundToInt(Mathf.RoundToInt(Mathf.Abs(_yMax - _yMin) / _tileSize.y)) + 1;
            
            _tiles = new T[_width, _height];
            foreach (var item in _tilesList)
            {
                RectTransform rectTransform = (RectTransform) item.transform;
                Point position = PositionToGrid(rectTransform.anchoredPosition, rectTransform.pivot);
                _tiles[position.X, position.Y] = item;
                item.Position = position;
            }
        }
    }
}