using UnityEngine;

public class Item : MonoBehaviour
{
    public int Level { get; private set; }
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

    public void SetLevel(int level)
    {
        Level = level;
    }
}