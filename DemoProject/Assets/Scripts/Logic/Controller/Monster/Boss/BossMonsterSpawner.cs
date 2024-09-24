using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterSpawner
{
    private const int MAX_SPAWN_COUNT = 10;

    private int _spawnCount = 1;

    public void SpawnMonster()
    {
        _spawnCount = Random.Range(1, MAX_SPAWN_COUNT);

        List<Vector3Int> tiles = TilemapHelper.GetTilesByLayer(TileLayer.Floor);

        List<int> cellIndex = CalcRandomTileIndex(tiles);

        for (int i = 0; i < cellIndex.Count; i++)
        {
            Vector3 cellToWorldPos = TilemapHelper.CellToWorldPos(TileLayer.Floor, tiles[cellIndex[i]]);

            // TODO
            // cellToWorldPos 에 몬스터 프리팹 소환 연출하기
        }
    }

    private List<int> CalcRandomTileIndex(List<Vector3Int> tiles)
    {
        List<int> randomCellIndex = new List<int>();

        for (int i = 0; i <= _spawnCount;)
        {
            int cellIndex = Random.Range(0, tiles.Count);

            if (randomCellIndex.Contains(cellIndex))
                continue;

            randomCellIndex.Add(cellIndex);
            i++;
        }

        return randomCellIndex;
    }
}
