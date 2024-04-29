using UnityEngine;

public class CardShape : ACardComponent
{
    [SerializeField] private SpriteRenderer[] _sprites;

    public void SetShape(Shape shape)
    {
        SpriteRenderer current;
        for (int i = 0; i < Shape.COUNT; i++) 
        {
            (current = _sprites[i]).color = shape.Color;
            current.sprite = shape.Value[i];
        }
    }

    public override void SetSize(Vector2 size)
    {
        for (int i = 0; i < Shape.COUNT; i++)
            _sprites[i].size = size;
    }

    public void ResetAngle() => _thisTransform.localRotation = Quaternion.identity;
}
