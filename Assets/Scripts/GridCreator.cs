using System;
using System.Collections;
using System.Collections.Generic;
using AudioContent;
using DG.Tweening;
using SOContent;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private GridConfig _gridConfig;
    [SerializeField] private Transform _container;

    private Cell[,] _grid;

    public event Action _GridCreated;

    public void CreateGrid()
    {
        _grid = new Cell[_gridConfig.Rows, _gridConfig.Cols];
        StartCoroutine(GenerateGridCoroutine());
    }

    private IEnumerator GenerateGridCoroutine()
    {
        for (int y = 0; y < _gridConfig.Rows; y++)
        for (int x = 0; x < _gridConfig.Cols; x++)
        {
            float posX = x * _gridConfig.XOffset;
            float posZ = y * _gridConfig.ZOffset;

            Vector3 finalPos = new Vector3(posX, 0, posZ);
            Vector3 startPos = finalPos + new Vector3(0, -1f, 0);

            Cell newCell = Instantiate(_gridConfig.CellPrefab, startPos, Quaternion.identity, _container);
            AudioPlayer.PlayCellSpawnSound();
            newCell.Init(x, y);
            _grid[y, x] = newCell;
            newCell.transform
                .DOMove(finalPos, 0.4f)
                .SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.05f);
        }

        _GridCreated?.Invoke();
    }

    public Cell GetFreeCell()
    {
        var empty = new List<Cell>();

        foreach (var c in _grid)
            if (c.IsEmpty)
                empty.Add(c);

        if (empty.Count == 0) return null;

        return empty[Random.Range(0, empty.Count)];
    }
}