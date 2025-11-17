using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;

    private Item _currentItem;
    // public bool IsEmpty => currentItem == null;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetItem(Item item)
    {
        _currentItem = item;
        
        if (item != null)
            item.transform.position = transform.position;
    }

    public void Clear()
    {
        _currentItem = null;
    }
}