using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;

    public Item CurrentItem{ get; private set; }
    public bool IsEmpty => CurrentItem == null;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetItem(Item item)
    {
        CurrentItem = item;
        
        if (item != null)
            item.transform.position = transform.position;
    }

    public void Clear()
    {
        CurrentItem = null;
    }
}