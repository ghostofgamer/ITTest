using UnityEngine;

namespace SOContent
{
    [CreateAssetMenu(fileName = "GridConfig", menuName = "ScriptableObjects/GridConfig", order = 1)]
    public class GridConfig : ScriptableObject
    {
        [SerializeField] private int _rows;
        [SerializeField] private int _cols;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private float _xOffset = 1f;
        [SerializeField] private float _zOffset = 1f;

        public int Rows => _rows;
        public int Cols => _cols;
        public Cell CellPrefab => _cellPrefab;
        public float XOffset => _xOffset;
        public float ZOffset => _zOffset;
    }
}