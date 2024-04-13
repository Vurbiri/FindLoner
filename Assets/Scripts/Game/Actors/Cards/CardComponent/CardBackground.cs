using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardBackground : ACardComponent
{
    [SerializeField] private Image _border;
    [SerializeField] private float _pixelSizeDefault = 0.25f;

    public IEnumerator Rotation90Angle_Coroutine(Vector3 axis, float speed)
    {
        float angle = 0, rotation;
        Quaternion target = _thisTransform.rotation * Quaternion.Euler(axis * 90f);

        while (angle < 90f) 
        {
            yield return null;

            rotation = speed * Time.deltaTime;
            _thisTransform.rotation *= Quaternion.Euler(axis * rotation);
            angle += rotation;
        }

        _thisTransform.rotation = target;
    }

    public void SetColorBorder(Color color) => _border.color = color;

    public void SetPixelSize(float ratio) =>
        _thisImage.pixelsPerUnitMultiplier = _border.pixelsPerUnitMultiplier = 1 + _pixelSizeDefault * ratio;
}
