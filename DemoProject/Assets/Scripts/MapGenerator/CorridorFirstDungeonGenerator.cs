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

    // PCG Data
    private Dictionary<Vector2Int, HashSet<Vector2Int>> _roomsDictionary
        = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

    private HashSet<Vector2Int> _floorPositions, _corridorPositions;

    // Gizmos Data
    private List<Color> _roomColors = new List<Color>();
    [SerializeField] private bool _showRoomGizmo = false;
    [SerializeField] private bool _showCorridorGizmo;
    
    // public List<Room> Rooms = new List<Room>();

    protected override void RunProceduralGeneration()
    {
        CorridorFirstDungeonGeneration();
    }

    private void CorridorFirstDungeonGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        // Corridors 생성
        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        // room이 시작될 수 있는 포지션에 rooomPercent 만큼 room 생성
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        // 끝 쪽에 방이 없는 corridor의 floor 위치들 탐색
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        // 끝 쪽에 Room이 없는 Corridor의 위치에 방추가
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        // 여태껏 만든 corridor들과 room 들을 결합
        floorPositions.UnionWith(roomPositions);

        for (int i = 0; i < corridors.Count; i++)
        {
            // corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }

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
        ClearRoomData();
        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            
            SaveRoomData(roomPosition, roomFloor);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private void ClearRoomData()
    {
        _roomsDictionary.Clear();
        _roomColors.Clear();
    }

    private void SaveRoomData(Vector2Int roomPosition, HashSet<Vector2Int> roomFloor)
    {
        _roomsDictionary[roomPosition] = roomFloor;
        _roomColors.Add(UnityEngine.Random.ColorHSV());

        // Rooms.Add(new Room(roomPosition, roomFloor));
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = _startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < _corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, _corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];     // Last Position of corridor
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }

        _corridorPositions = new HashSet<Vector2Int>(floorPositions);
        return corridors;
    }

    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previousDirection = Vector2Int.zero;
        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (previousDirection != Vector2Int.zero && 
                directionFromCell != previousDirection)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                previousDirection = directionFromCell;
            }
            else
            {
                Vector2Int newCorridorTileOffset
                    = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }
        }
        return newCorridor;
    }

    public List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                }
            }
        }
        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return Vector2Int.right;
        if (direction == Vector2Int.right) 
            return Vector2Int.down;
        if (direction == Vector2Int.down)
            return Vector2Int.left;
        if (direction == Vector2Int.left)
            return Vector2Int.up;

        Debug.LogError("GetDirection90From : Direction parameter has invalid format.");
        return Vector2Int.zero;
    }
}
