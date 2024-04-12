using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class ACardComponent : MonoBehaviour
{
    protected Image _thisImage;
    protected Transform _thisTransform;

    private void Awake()
    {
        _thisImage = GetComponent<Image>();
        _thisTransform = transform;
    }

    public void SetColor(Color color) => _thisImage.color = color;

    public void Mirror(Vector3 axis) => _thisTransform.rotation *= Quaternion.Euler(axis * 180f);
    public void Rotation(Vector3 axis, float angle) => _thisTransform.rotation *= Quaternion.Euler(axis * angle);
}
