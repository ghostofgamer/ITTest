using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private Item _itemPrefab;
    [SerializeField] private GridCreator _grid;
    [SerializeField] private Transform _container;

    private void OnEnable()
    {
        _grid._GridCreated += StartSpawn;
    }

    private void OnDisable()
    {
        _grid._GridCreated -= StartSpawn;
    }

    private void StartSpawn()
    {
        InvokeRepeating(nameof(SpawnItem), 1f, _spawnInterval);
    }

    void SpawnItem()
    {
        var cell = _grid.GetFreeCell();
        if (cell == null) return;

        Item it = Instantiate(_itemPrefab, _container);
        it.SetLevel(1);
        it.SetCell(cell);
    }
}