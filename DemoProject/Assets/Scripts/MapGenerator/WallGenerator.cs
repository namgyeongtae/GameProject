using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionList);
        foreach (var position in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionsList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions) 
        {
            foreach (var direction in directionsList)
            {
                var neighborPosition = position + direction;
                // 각각의 타일마다 상하좌우 타일 확인 후 floor에 포함되지 않으면 
                // 가장 외부 floor 타일의 이웃 타일이므로 거기를 wall 로 인식
                if (floorPositions.Contains(neighborPosition) == false)
                    wallPositions.Add(neighborPosition);
            }
        }

        return wallPositions;
    }
}
