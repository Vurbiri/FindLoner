using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ACardComponent : MonoBehaviour
{
    protected Image _thisImage;
    protected Transform _thisTransform;

    private void Awake()
    {
        _thisImage = GetComponent<Image>();
        _thisTransform = transform;
    }

    public void SetColor(Color color) => _thisImage.color = color;
    public void SetColorFade(Color color, float duration) => _thisImage.CrossFadeColor(color, duration, false, false);


    public void Rotation(Vector3 axis, float angle)
    {
        _thisTransform.rotation *= Quaternion.Euler(axis * angle);
    }
}
