using UnityEngine;

public class DragItemMerge : MonoBehaviour
{
    [Header("Drag settings")] [SerializeField]
    private float liftY = 2f;

    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private int _maxLevel = 5;
    [SerializeField] private ItemSpawner _itemSpawner;

    private RaycastHit hit;
    private Item _selectObject;
    private Vector3 _startPosition;
    private Cell _currentCell;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            TakeItem();

        if (_selectObject == null)
            return;
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Join();
            _selectObject = null;
        }

        if (_selectObject != null)
            Move();
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
                    return;
                }
            }
        }

        ResetPosition();
    }

    private void ResetPosition()
    {
        _selectObject.transform.position = _startPosition;
    }

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
            ResetPosition();
            return;
        }

        if (cell != _selectObject.Cell)
        {
            Debug.Log("Чужое CEll");

            if (_selectObject.Level == cell.CurrentItem.Level)
            {
                Debug.Log("у них один уровень");
                _selectObject.gameObject.SetActive(false);
                cell.CurrentItem.gameObject.SetActive(false);
                _selectObject.Cell.Clear();
                ClearCell();
                
                _itemSpawner.MergeSpawn(cell,_selectObject.Level + 1);
            }
            else
            {
                ResetPosition();
                Debug.Log("Разные уровни");
            }
        }
        else
        {
            ResetPosition();
            Debug.Log("Наше CEll");
        }
    }
}