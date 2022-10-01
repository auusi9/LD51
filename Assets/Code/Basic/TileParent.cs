using System.Drawing;
using UnityEngine;

namespace Code.Basic
{
    public abstract class TileParent<T> : MonoBehaviour where T : Tile
    {
        [SerializeField] private T[] _tilesList;
        [SerializeField] private Vector2 _padding;
        [SerializeField] private Vector2 _tileSize;
        [SerializeField] private Vector2 _spacing;
        [SerializeField] private int _width, _height;

        protected T[,] _tiles;

        public int Width => _width;
        public int Height => _height;

        private void Start()
        {
            _tiles = new T[_width, _height];
            foreach (var item in _tilesList)
            {
                RectTransform rectTransform = (RectTransform) item.transform;
                Point position = PositionToGrid(rectTransform.anchoredPosition, rectTransform.pivot);
                _tiles[position.X, position.Y] = item;
                item.Position = position;
            }
        }

        private Point PositionToGrid(Vector2 position, Vector2 pivot)
        {
            float percentX = (position.x - _padding.x - _tileSize.x * pivot.x) / ((_width-1) * _tileSize.x + (_width-1) * _spacing.x);
            float percentY = (Mathf.Abs(position.y) - _padding.y - _tileSize.y * pivot.y) / ((_height-1) * _tileSize.y + (_height-1) * _spacing.y);
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
    }
}