using UnityEngine;


namespace Player
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform playerTransform;
        
        [SerializeField] private float cameraSpeed = 0.1f;

        [SerializeField] private Vector3 offset;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void LateUpdate()
        {
            transform.position = new Vector3(
                playerTransform.position.x + offset.x,
                playerTransform.position.y + offset.y,
                transform.position.z  
            );
        }
    }
}