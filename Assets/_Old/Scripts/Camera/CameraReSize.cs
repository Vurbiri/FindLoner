using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraReSize : MonoBehaviour
{
    private Camera _thisCamera;
    private float aspectRatioOld = 0f;
    private Vector2 _halfSize = Vector2.zero;

    public event Action<Vector2> EventReSize;
    
    private void Awake()
    {
        _thisCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (aspectRatioOld == _thisCamera.aspect)
            return;

        aspectRatioOld = _thisCamera.aspect;
        _halfSize.x = _thisCamera.orthographicSize * aspectRatioOld;
        _halfSize.y = _thisCamera.orthographicSize;

        EventReSize?.Invoke(_halfSize);
    }
}
