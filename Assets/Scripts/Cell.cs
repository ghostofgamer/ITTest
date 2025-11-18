using UnityEngine;

public class Cell : MonoBehaviour
{
    public Item CurrentItem{ get; private set; }
    public bool IsEmpty => CurrentItem == null;

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