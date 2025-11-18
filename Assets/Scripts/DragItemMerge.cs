using UnityEngine;

public class DragItemMerge : MonoBehaviour
{
    [Header("Drag settings")] [SerializeField]
    private float liftY = 2f; // насколько поднимается объект

    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private int _maxLevel = 5;
    [SerializeField] private ItemSpawner _itemSpawner;

    [Header("Item properties")] public int Level = 1; // уровень элемента
    public int Id;

    private Camera mainCam;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 startPosition;
    private RaycastHit hit;
    private Item _selectObject;
    private Vector3 _startPosition;

    static int cellLayerNumber = 6;
    LayerMask cellLayerMask = 1 << cellLayerNumber;


    private Cell _currentCell;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            TakeItem();

        if (_selectObject == null)
            return;

        /*if (isDragging)
        {
            FollowMouse();
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            ReleaseItem();
        }*/

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Join();

            _selectObject = null;
        }

        if (_selectObject != null)
        {
            Move();
        }
    }

    private void Join()
    {
        Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.WorldToScreenPoint(_selectObject.transform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector3 rayDirection = worldPos - Camera.main.transform.position;
        RaycastHit hitInfo;


        if (Physics.Raycast(worldPos, rayDirection, out hitInfo, Mathf.Infinity, cellLayer))
        {
            Debug.Log("Hit " + hitInfo.transform.name);

            if (hitInfo.transform.TryGetComponent(out Cell cell))
            {
                if (cell.IsEmpty)
                {
                    ClearCell();
                    _selectObject.SetCell(cell);
                    Debug.Log("Cell is Empty!");
                    return;
                }
                else
                {
                    ComparisonItem(cell);
                    Debug.Log("Cell is НЕЕЕ Empty!");
                    // return;
                }
            }
        }

        ResetPosition();

        /*if (Physics.Raycast(worldPos, rayDirection, out hitInfo, Mathf.Infinity, cellLayerMask))
        {
            if (hitInfo.transform.TryGetComponent(out Cell cell))
            {
                Debug.Log("Cell ");
                /*if (!cell.IsStay)
                {
                    // Snap к клетке
                    _selectObject.transform.position = cell.transform.position;
                    _startPosition = cell.transform.position;
                }
                else
                {
                    // Если клетка занята — Merge
                    SetNewTank(hitInfo);
                }#1#

                // Всё, больше ничего делать не нужно
                ResetPosition();
                return;
            }
        }*/

        /*if (Physics.Raycast(worldPosition, rayDirection, out hitInfo, Mathf.Infinity, _layerMask))
        {
            if (hitInfo.transform.TryGetComponent(out Cell positionTank))
            {
                if (!hitInfo.collider.gameObject.GetComponent<Cell>().IsStay)
                {
                    _selectObject.transform.position = hitInfo.transform.position;
                    _startPosition = hitInfo.collider.gameObject.transform.position;
                }

                if (hitInfo.collider.gameObject.GetComponent<Cell>().IsStay)
                {
                    SetNewTank(hitInfo);
                }

                return;
            }
            else
            {
                ResetPosition();
                return;
            }
        }*/
    }

    private void ResetPosition()
    {
        _selectObject.transform.position = _startPosition;
    }

    #region Drag logic

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosfar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);

        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        Vector3 worldPosMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosfar);
        Vector3 worldPosMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldPosMousePosNear, worldPosMousePosFar - worldPosMousePosNear, out hit);
        return hit;
    }

    private void ClearCell()
    {
        _currentCell.Clear();
        _currentCell = null;
    }

    private void TakeItem()
    {
        if (_selectObject == null)
        {
            hit = CastRay();

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out Item dragItem))
                {
                    _currentCell = dragItem.Cell;
                    _selectObject = dragItem;
                    _startPosition = _selectObject.transform.position;
                }
            }
        }
    }

    private void Move()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.WorldToScreenPoint(_selectObject.transform.position).z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        Vector3 currentPosition = new Vector3(worldPosition.x, 1.65f, worldPosition.z);
        _selectObject.transform.position =
            Vector3.Lerp(_selectObject.transform.position, currentPosition, 10 * Time.deltaTime);
    }

    private void ComparisonItem(Cell cell)
    {
        if (_selectObject.Level >= _maxLevel)
        {
            Debug.Log("левел максимальный " + _selectObject.Level);
            return;
        }

        if (cell != _selectObject.Cell)
        {
            Debug.Log("Чужое CEll");

            if (_selectObject.Level == cell.CurrentItem.Level)
            {
                Debug.Log("у них один уровень");
                Item item = _itemSpawner.GetItem(_selectObject.Level + 1);

                if (item != null)
                {
                    _selectObject.gameObject.SetActive(false);
                    cell.CurrentItem.gameObject.SetActive(false);
                    
                    // cell.Clear();
                    _selectObject.Cell.Clear();
                    ClearCell();
                    
                    Item it = Instantiate(item, transform);
                    it.SetCell(cell);
                }
            }
            else
            {
                Debug.Log("Разные уровни");
            }
        }
        else
        {
            Debug.Log("Наше CEll");
        }
    }


    private void ReleaseItem()
    {
        isDragging = false;

        // Ищем ближайшую Cell через Raycast
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cellLayer))
        {
            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell != null)
            {
                if (cell.IsEmpty)
                {
                    SnapToCell(cell);
                }
                else
                {
                    // если на Cell уже есть Item того же уровня → Merge
                    DragItemMerge otherItem = cell.CurrentDragItemMerge;

                    if (otherItem != null && otherItem.Level == this.Level && otherItem.Id != this.Id)
                    {
                        MergeWith(otherItem, cell);
                    }
                    else
                    {
                        // вернуть на стартовую позицию
                        transform.position = startPosition;
                    }
                }
            }
        }
        else
        {
            // не попал на Cell → вернуть
            transform.position = startPosition;
        }
    }

    #endregion

    #region Helper methods

    private void SnapToCell(Cell cell)
    {
        transform.position = cell.transform.position;
        cell.CurrentDragItemMerge = this;
    }

    private void MergeWith(DragItemMerge other, Cell cell)
    {
        /*// создаём новый Item следующего уровня на Cell
        int newLevel = this.Level + 1;

        GameObject newItemPrefab = MergeManager.Instance.GetPrefab(newLevel);
        GameObject newItem = Instantiate(newItemPrefab, cell.transform.position, Quaternion.identity);

        // присваиваем уровень и Id
        DragItemMerge newDragItem = newItem.GetComponent<DragItemMerge>();
        newDragItem.Level = newLevel;
        newDragItem.Id = Random.Range(1, 999999);

        // деактивируем старые Items
        Destroy(this.gameObject);
        Destroy(other.gameObject);

        // назначаем на Cell новый Item
        cell.CurrentItem = newDragItem;*/
    }

    #endregion
}