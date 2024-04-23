using UnityEngine;

public class CardShape : ACardComponent
{
    [SerializeField] private SpriteRenderer _centerSprite;
    [SerializeField] private SpriteRenderer _topSprite;
    [SerializeField] private SpriteRenderer _leftSprite;
    [SerializeField] private SpriteRenderer _bottomSprite;
    [SerializeField] private SpriteRenderer _rightSprite;

    public void SetShape(Shape shape)
    {
        _thisSprite.sprite = shape.Main;
        _thisSprite.color = shape.Color;

        _centerSprite.sprite = shape.Center;
        _centerSprite.color = shape.Color;

        _topSprite.sprite = shape.Top;
        _topSprite.color = shape.Color;

        _leftSprite.sprite = shape.Left;
        _leftSprite.color = shape.Color;

        _bottomSprite.sprite = shape.Bottom;
        _bottomSprite.color = shape.Color;

        _rightSprite.sprite = shape.Right;
        _rightSprite.color = shape.Color;
    }

    public override void SetSize(Vector2 size)
    {
        _thisSprite.size = size;
        _centerSprite.size = size;
        _topSprite.size = size;
        _leftSprite.size = size;
        _bottomSprite.size = size;
        _rightSprite.size = size;
    }

    public void ResetAngle() => _thisTransform.localRotation = Quaternion.identity;
}
