using System;
using UnityEngine;

public class Kuvitoenmaincra : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    private void Update()
    {
        transform.position += Vector3.right * speed;
    }
}
