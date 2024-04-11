using UnityEngine;

public class CardShape : ACardComponent
{
    public void SetShape(Shape shape)
    {
        _thisImage.sprite = shape.Sprite;
        _thisImage.color = shape.Color;
    }

    public void Mirror(Vector3 axis) => Rotation(axis, 180f);
}
