using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int _level;

    public int Level => _level;
    public Cell Cell { get; private set; }
    public int Id { get; private set; }

    private void Start()
    {
        Id = GetInstanceID();
    }

    public void SetCell(Cell cell)
    {
        Cell = cell;
        cell.SetItem(this);
    }
}