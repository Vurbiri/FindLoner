using UnityEngine;
using UnityEngine.UI;

public class CardShape : ACardComponent
{
    [SerializeField] private Image _centerImage;
    [SerializeField] private Image _topImage;
    [SerializeField] private Image _leftImage;
    [SerializeField] private Image _bottomImage;
    [SerializeField] private Image _rightImage;

    public void SetShape(Shape shape)
    {
        _thisImage.sprite = shape.Main;
        _thisImage.color = shape.Color;

        _centerImage.sprite = shape.Center;
        _centerImage.color = shape.Color;

        _topImage.sprite = shape.Top;
        _topImage.color = shape.Color;

        _leftImage.sprite = shape.Left;
        _leftImage.color = shape.Color;

        _bottomImage.sprite = shape.Bottom;
        _bottomImage.color = shape.Color;

        _rightImage.sprite = shape.Right;
        _rightImage.color = shape.Color;
    }

    public void ResetAngle() => _thisTransform.localRotation = Quaternion.identity;
}
