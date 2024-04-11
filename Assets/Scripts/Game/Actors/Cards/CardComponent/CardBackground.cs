using System.Collections;
using UnityEngine;

public class CardBackground : ACardComponent
{
    [SerializeField] private float _pixelSizeDefault = 0.25f;

    public IEnumerator Rotation90AngleCoroutine(Vector3 axis, float speed)
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

    public void SetPixelSize(float ratio) => _thisImage.pixelsPerUnitMultiplier = 1 + _pixelSizeDefault * ratio;
}
