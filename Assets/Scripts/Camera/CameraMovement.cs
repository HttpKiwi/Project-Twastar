using System;
using UnityEngine;


namespace Camera
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform playerTransform;
        public float leftLimit = 0f;
        public float bottomLimit = 0f;
        public float rightLimit = 128f;
        public float topLimit = 64f; 
        
        private Vector3 _cameraPosition;
        private UnityEngine.Camera _camera;
        
        void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            
            float cameraHalfHeight = _camera.orthographicSize;
            float cameraHalfWidth = cameraHalfHeight * _camera.aspect;
                
            leftLimit += cameraHalfWidth;
            rightLimit -= cameraHalfWidth;
            bottomLimit += cameraHalfHeight;
            topLimit -= cameraHalfHeight;
            
            _cameraPosition.Set(
                Math.Clamp(playerTransform.position.x, leftLimit, rightLimit),
                Math.Clamp(playerTransform.position.y, bottomLimit, topLimit),
                transform.position.z
            );
            transform.position = _cameraPosition;
        }
        
        void LateUpdate()
        {
            _cameraPosition.Set(
                Math.Clamp(playerTransform.position.x, leftLimit, rightLimit),
                Math.Clamp(playerTransform.position.y, bottomLimit, topLimit),
                transform.position.z
            );
            transform.position = _cameraPosition;
            
        }
    }
}