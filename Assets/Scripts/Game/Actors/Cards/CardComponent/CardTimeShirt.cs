using UnityEngine;

public class CardTimeShirt : MonoBehaviour
{
    private Transform _thisTransform;

    private void Awake()
    {
        _thisTransform = transform;
    }

    public void SetActive(bool active) => gameObject.SetActive(active);

    public void Set180Angle(Vector3 axis) => _thisTransform.localRotation = Quaternion.Euler(axis * 180);
}
