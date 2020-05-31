using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;

    private Transform _transform;
    private float initialY;


    private void Awake()
    {
        _transform = transform;
        initialY = _transform.position.y;
    }

    private void LateUpdate()
    {
        Vector2 camPos = new Vector2(mainCamera.position.x, mainCamera.position.z);
        _transform.position = new Vector3(camPos.x, initialY, camPos.y);
    }
}
