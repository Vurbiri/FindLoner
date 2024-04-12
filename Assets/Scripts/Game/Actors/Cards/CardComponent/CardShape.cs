using UnityEngine;

public class CardShape : ACardComponent
{
    public void SetShape(Shape shape)
    {
        _thisImage.sprite = shape.Sprite;
        _thisImage.color = shape.Color;
    }
}
