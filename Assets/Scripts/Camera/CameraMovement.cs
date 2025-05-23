using UnityEngine;

namespace Player
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float cameraSpeed = 0.1f;

        [SerializeField] private Vector3 offset;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, PlayerMovement.instance.transform.position + offset, cameraSpeed);
        }
    }
}