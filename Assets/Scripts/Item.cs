using UnityEngine;

public class Item : MonoBehaviour
{
    public int Level { get; private set; }
    public Cell Cell{ get; private set; }

    public void SetCell(Cell cell)
    {
        this.Cell = cell;
        cell.SetItem(this);
    }

    public void SetLevel(int level)
    {
        this.Level = level;
    }
}