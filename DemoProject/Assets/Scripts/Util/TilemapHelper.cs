using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileLayer
{
    None,
    Floor,
    Wall,
    Deco
}

public class TilemapHelper
{
    public static List<Vector3Int> GetTilesByLayer(TileLayer layer)
    {
        List<Vector3Int> cells = new List<Vector3Int>();

        Tilemap tilemap = Managers.TileMap.WorldTilemap[layer];

        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                if (tilemap.HasTile(cellPosition))
                {
                    cells.Add(cellPosition);
                }
            }
        }

        return cells;
    }

    public static Vector3 CellToWorldPos(TileLayer layer, Vector3Int cell)
    {
        Tilemap tilemap = Managers.TileMap.WorldTilemap[layer];

        if (!tilemap.HasTile(cell))
        {
            Debug.Log($"{layer.ToString()} has no cell of ({cell.x}, {cell.y}, {cell.z})");
            return Vector3.negativeInfinity;
        }

        return tilemap.CellToWorld(cell);
    }
}
