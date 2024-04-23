using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraReSize : MonoBehaviour
{
    [SerializeField] private CanvasScaler _canvasScaler;

    private Camera _thisCamera;
    private float aspectRatioOld = 0f;
    private float _verticalHalfSizeMin = 500f;
    private float _horizontalHalfSizeMin = 800f;

    private void Awake()
    {
        _thisCamera = GetComponent<Camera>();
        _verticalHalfSizeMin = _canvasScaler.referenceResolution.y / 2f;
        _horizontalHalfSizeMin = _canvasScaler.referenceResolution.x / 2f;
    }

    private void Update()
    {
        if (aspectRatioOld == _thisCamera.aspect)
            return;

        aspectRatioOld = _thisCamera.aspect;
        float horizontalHalfSize = _verticalHalfSizeMin * aspectRatioOld;

        if (horizontalHalfSize < _horizontalHalfSizeMin)
            _thisCamera.orthographicSize = _horizontalHalfSizeMin / aspectRatioOld;
        else
            _thisCamera.orthographicSize = _verticalHalfSizeMin;

        //Debug.Log((_thisCamera.orthographicSize * aspectRatioOld * 2f) + " - " + (_thisCamera.orthographicSize * 2f));
    }
}
