using System.Collections.Generic;
using System.Drawing;
using Code.Basic;
using Code.Items;

namespace Code.Boxes
{
    public class Box : TileParent<BoxTile>
    {
        public bool Fits(Item item, BoxTile boxTile, out List<BoxTile> boxTiles)
        {
            boxTiles = new List<BoxTile>();
            ItemTile[,] getTiles = item.GetTiles();

            if (Width < item.Width || Height < item.Height)
            {
                return false;
            }

            Point initialPosition = boxTile.Position;
            int width = item.Width-1;
            int height = item.Height-1;
            
            for(int i = 0; i < item.TiledWidth; i++)
            {
                for(int j = 0; j < item.TiledHeight; j++)
                {
                    if (getTiles[i, j] != null)
                    {
                        BoxTile tile = GetTileAtPosition(initialPosition.X + i - width, initialPosition.Y + j - height);
                        
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
        }
    }
}