using System.Collections.Generic;
using UnityEngine;

namespace SpawnContent
{
    public class ItemSpawner : MonoBehaviour
    {
// @formatter:off           
        [Header("References")]
        [SerializeField] private GridCreator _grid;
        [SerializeField] private Item[] _prefabs;
        [SerializeField] private Transform _container;
        
        [Header("Parameters")]
        [SerializeField] private float _spawnInterval = 3f;
// @formatter:on

        private Item _item;
        private int _defaultLevel = 1;
        private Dictionary<int, ObjectPool<Item>> _pools;

        private void OnEnable()
        {
            _grid.GridCreated += StartSpawn;
        }

        private void OnDisable()
        {
            _grid.GridCreated -= StartSpawn;
        }

        public void Init()
        {
            InitPools();
        }

        public void MergeSpawn(Cell cell, int targetLvl)
        {
            _item = GetFromPool(targetLvl);

            if (_item != null)
            {
                _item.SetCell(cell);
                _item.PlayParticle();
            }
            else
            {
                Debug.Log("Ошибка поиска префаба");
            }
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

            Item it = GetFromPool(_defaultLevel);
            it.SetCell(cell);
        }
    }
}