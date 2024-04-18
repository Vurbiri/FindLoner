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

    public void Mirror(Vector3 axis) => _thisTransform.rotation *= Quaternion.Euler(axis * 180f);
    public void Set90Angle(Vector3 axis) => _thisTransform.localRotation = Quaternion.Euler(axis * 90);
    public void Rotation(Vector3 axis, float angle) => _thisTransform.rotation *= Quaternion.Euler(axis * angle);
}
