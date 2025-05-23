using UnityEngine;


namespace Camera
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform playerTransform;
        
        [SerializeField] private float cameraSpeed = 0.1f;

        [SerializeField] private Vector3 offset;
        [SerializeField] private float cameraAreaPosition = 3f;
        [SerializeField] private float cameraLimitArea = 0.2f;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerTransform.position, 1);
            Gizmos.DrawWireSphere(transform.position, cameraAreaPosition);
        }
        
        private void LateUpdate()
        {

            float xPosition = 0f;
            float yPosition = 0f;
            if (playerTransform.position.x >= transform.position.x + cameraAreaPosition - cameraLimitArea)
            {
                xPosition += cameraLimitArea;
            }
            else if (playerTransform.position.x <= transform.position.x - cameraAreaPosition + cameraLimitArea)
            {
                xPosition -= cameraLimitArea;
            }
            if (playerTransform.position.y >= transform.position.y + cameraAreaPosition - cameraLimitArea)
            {
                yPosition += cameraLimitArea;
            }
            else if (playerTransform.position.y <= transform.position.y - cameraAreaPosition + cameraLimitArea)
            {
                yPosition -= cameraLimitArea;
            }
            transform.position = new Vector3(
                transform.position.x + xPosition,
                transform.position.y + yPosition,
                transform.position.z  
            );
        }
    }
}