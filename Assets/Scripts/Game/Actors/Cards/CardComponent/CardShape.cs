using UnityEngine;

public class CardShape : ACardComponent
{
    public void SetShape(Shape shape)
    {
        _thisImage.sprite = shape.Value;
        _thisImage.color = shape.Color;
    }

    public void SetColor(Color color) => _thisImage.color = color;
    public void ResetAngle() => _thisTransform.localRotation = Quaternion.identity;
}
