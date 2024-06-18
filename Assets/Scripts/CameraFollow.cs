using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Референс на компонент transform гравця
    public float smoothSpeed = 0.125f; // значення зглажування руху камери
    public Vector3 offset; // відхилення камери від гравця

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z) + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
