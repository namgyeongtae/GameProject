using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap _floorTilemap;
    [SerializeField] private Tilemap _wallTilemap;

    [SerializeField] private TileBase _floorTile;
    [SerializeField] private TileBase _wallTop, _wallSideRight, _wallSideLeft, _wallBottom, _wallFull;
    [SerializeField] private TileBase _wallInnerCornerDownLeft, _wallInnerCornerDownRight, 
                                      _wallDiagonalCornerDownRight, _wallDiagonalCornerDownLeft, 
                                      _wallDiagonalCornerUpRight, _wallDiagonalCornerUpLeft;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, _floorTilemap, _floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        _wallTilemap.ClearAllTiles();
        _floorTilemap.ClearAllTiles();
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallByteTypes.wallTop.Contains(typeAsInt))
        {
            tile = _wallTop;
        }
        else if (WallByteTypes.wallSideRight.Contains(typeAsInt))
        {
            tile = _wallSideRight;
        }
        else if (WallByteTypes.wallSideLeft.Contains(typeAsInt))
        {
            tile = _wallSideLeft;
        }
        else if (WallByteTypes.wallBottm.Contains(typeAsInt))
        {
            tile = _wallBottom;
        }
        else if (WallByteTypes.wallFull.Contains(typeAsInt))
        {
            tile = _wallFull;
        }
        if (tile != null)
            PaintSingleTile(_wallTilemap, tile, position);
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallByteTypes.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = _wallInnerCornerDownLeft;
        }
        else if (WallByteTypes.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = _wallInnerCornerDownRight;
        }
        else if (WallByteTypes.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = _wallDiagonalCornerDownLeft;
        }
        else if (WallByteTypes.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = _wallDiagonalCornerDownRight;
        }
        else if (WallByteTypes.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = _wallDiagonalCornerUpRight;
        }
        else if (WallByteTypes.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = _wallDiagonalCornerUpLeft;
        }
        else if (WallByteTypes.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = _wallFull;
        }
        else if (WallByteTypes.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = _wallBottom;
        }

        if (tile != null)
            PaintSingleTile(_wallTilemap, tile, position);
    }
}
