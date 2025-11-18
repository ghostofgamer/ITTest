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
    private Vector3 _finalPosition;
    private Vector3 _startPosition;
    private float _posX;
    private float _posZ;
    private Coroutine _gridCoroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.05f);

    public event Action GridCreated;

    public void CreateGrid()
    {
        _grid = new Cell[_gridConfig.Rows, _gridConfig.Cols];

        if (_gridCoroutine != null)
            StopCoroutine(_gridCoroutine);

        _gridCoroutine = StartCoroutine(GenerateGridCoroutine());
    }

    private IEnumerator GenerateGridCoroutine()
    {
        for (int y = 0; y < _gridConfig.Rows; y++)
        for (int x = 0; x < _gridConfig.Cols; x++)
        {
            _posX = x * _gridConfig.XOffset;
            _posZ = y * _gridConfig.ZOffset;
            _finalPosition = new Vector3(_posX, 0, _posZ);
            _startPosition = _finalPosition + new Vector3(0, -1f, 0);
            Cell newCell = Instantiate(_gridConfig.CellPrefab, _startPosition, Quaternion.identity, _container);
            AudioPlayer.PlayCellSpawnSound();
            _grid[y, x] = newCell;
            newCell.transform.DOMove(_finalPosition, 0.4f).SetEase(Ease.OutBack);
            yield return _waitForSeconds;
        }

        GridCreated?.Invoke();
    }

    public Cell GetFreeCell()
    {
        var empty = new List<Cell>();

        foreach (var c in _grid)
            if (c.IsEmpty)
                empty.Add(c);

        if (empty.Count == 0)
            return null;

        return empty[Random.Range(0, empty.Count)];
    }
}