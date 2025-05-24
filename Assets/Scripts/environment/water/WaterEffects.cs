using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace environment.water
{
    public class WaterEffects : MonoBehaviour
    {
        
        public float waveSpeedY = 2f;
        public float waveSpeedX = 1f;
        public float waveHeight = 0.5f;
        
        
        private Vector3 _startPosition;

        void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            float newY = _startPosition.y + (Mathf.Sin(Time.time * waveSpeedY) * waveHeight);
            float newX = _startPosition.x + (Mathf.Cos(Time.time * waveSpeedX) * waveHeight);
            transform.position = new Vector3(newX, newY, _startPosition.z);
        }
    }
}