using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private GridCreator _grid;
    [SerializeField] private Transform _container;
    [SerializeField] private Item[] _prefabs;

    private Item _defaultItem;
    private Dictionary<int, ObjectPool<Item>> _pools;

    private void OnEnable()
    {
        _grid._GridCreated += StartSpawn;
    }

    private void OnDisable()
    {
        _grid._GridCreated -= StartSpawn;
    }

    public void Init()
    {
        InitPools();
    }

    public void MergeSpawn(Cell cell, int targetLvl)
    {
        Item item = GetFromPool(targetLvl);

        if (item != null)
        {
            item.SetCell(cell);
            item.PlayParticle();
        }
        else
            Debug.Log("Ошибка поиска префаба");
    }

    private void InitPools()
    {
        _pools = new Dictionary<int, ObjectPool<Item>>();

        foreach (var prefab in _prefabs)
        {
            if (!_pools.ContainsKey(prefab.Level))
            {
                var pool = new ObjectPool<Item>(prefab, 5, _container);
                pool.SetAutoExpand(true);

                _pools.Add(prefab.Level, pool);
            }
        }
    }

    private Item GetFromPool(int level)
    {
        if (_pools.TryGetValue(level, out var pool))
        {
            if (pool.TryGetObject(out Item item, pool.Prefab))
                return item;
        }

        Debug.LogError($"Пул для уровня {level} не найден или пуст");
        return null;
    }

    private void StartSpawn()
    {
        InvokeRepeating(nameof(SpawnItem), 1f, _spawnInterval);
    }

    private void SpawnItem()
    {
        var cell = _grid.GetFreeCell();

        if (cell == null)
            return;

        Item it = GetFromPool(1);
        it.SetCell(cell);
    }
}