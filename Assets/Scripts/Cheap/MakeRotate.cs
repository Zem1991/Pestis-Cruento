using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRotate : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        Vector3 eulers = rotation * speed * Time.deltaTime;
        transform.Rotate(eulers, Space.Self);
    }
}
