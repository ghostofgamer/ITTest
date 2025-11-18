using UnityEngine;

namespace UI
{
    public class IconRotator : MonoBehaviour
    {
// @formatter:off        
        [Header("Rotation Settings")]
        [SerializeField] private float _rotationSpeed = 180f; 
        [SerializeField] private Vector3 _rotationAxis = Vector3.forward;
// @formatter:on

        private void Update()
        {
            transform.Rotate(_rotationAxis * _rotationSpeed * Time.deltaTime);
        }
    }
}