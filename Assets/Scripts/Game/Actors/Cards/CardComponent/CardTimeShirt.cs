using UnityEngine;

public class CardTimeShirt : ACardComponent
{
    public void SetOrderInLayer(int id) => _thisSprite.sortingOrder = id;

    public void SetActive(bool active) => gameObject.SetActive(active);

    public void Set180Angle(Vector3 axis) => _thisTransform.localRotation = Quaternion.Euler(axis * 180);
}
