using UnityEngine;

public class CardTimeShirt : MonoBehaviour
{
    private Transform _thisTransform;

    private void Awake()
    {
        _thisTransform = transform;
    }

    public void SetActive(bool active) => gameObject.SetActive(active);

    public void Mirror(Vector3 axis) => _thisTransform.rotation = Quaternion.Euler(axis * 180);
}
