using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int _corridorLength = 14;
    [SerializeField] private int _corridorCount = 5;

    [SerializeField][Range(0.1f, 1)] private float _roomPercent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstDungeonGeneration();
    }

    private void CorridorFirstDungeonGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        // Corridors ����
        CreateCorridors(floorPositions, potentialRoomPositions);

        // room�� ���۵� �� �ִ� �����ǿ� rooomPercent ��ŭ room ����
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        // �� �ʿ� ���� ���� corridor�� floor ��ġ�� Ž��
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        // �� �ʿ� Room�� ���� Corridor�� ��ġ�� ���߰�
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        // ���²� ���� corridor��� room ���� ����
        floorPositions.UnionWith(roomPositions);

        _tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, _tilemapVisualizer);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (!roomFloors.Contains(position))
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach(var position in floorPositions)
        {
            int neighborsCount = 0;
            foreach(var direction in Direction2D.cardinalDirectionList)
            {
                if (floorPositions.Contains(position + direction))
                    neighborsCount++;
            }
            if (neighborsCount == 1)
                deadEnds.Add(position);
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * _roomPercent);
    
        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        
        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = _startPosition;
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i < _corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, _corridorLength);
            currentPosition = corridor[corridor.Count - 1];     // Last Position of corridor
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }
}
