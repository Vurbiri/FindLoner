using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraReSize : MonoBehaviour
{
    [SerializeField] private CanvasScaler _canvasScaler;

    private Camera _thisCamera;
    private float _aspectRatioOld = 0f;
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
        if (_aspectRatioOld == _thisCamera.aspect)
            return;

        _aspectRatioOld = _thisCamera.aspect;
        float horizontalHalfSize = _verticalHalfSizeMin * _aspectRatioOld;

        if (horizontalHalfSize < _horizontalHalfSizeMin)
            _thisCamera.orthographicSize = _horizontalHalfSizeMin / _aspectRatioOld;
        else
            _thisCamera.orthographicSize = _verticalHalfSizeMin;
    }
}
