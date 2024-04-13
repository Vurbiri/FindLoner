
using UnityEngine;

public class CardTimeShirt : ACardComponent
{
    public void SetActive(bool active) => gameObject.SetActive(active);

    public override void Mirror(Vector3 axis)
    {
        _thisTransform.rotation = Quaternion.identity;
        base.Mirror(axis);
    }
}
