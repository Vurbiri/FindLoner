using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShape : ACardComponent
{
    [SerializeField] private Image _centerImage;
    [SerializeField] private Image _topImage;
    [SerializeField] private Image _leftImage;
    [SerializeField] private Image _bottomImage;
    [SerializeField] private Image _rightImage;

    private readonly List<Image> _images = new(COUNT);
    private const int COUNT = 6;

    protected override void Awake()
    {
        base.Awake();

        _images.Add(_thisImage);
        _images.Add(_centerImage);
        _images.Add(_topImage);
        _images.Add(_leftImage);
        _images.Add(_bottomImage);
        _images.Add(_rightImage);
    }

    public void SetShape(Shape shape)
    {
        Image temp;
        for (int i = 0; i < COUNT; i++) 
        {
            temp = _images[i];
            temp.sprite = shape.Value[i];
            temp.color = shape.Color;
        }
    }

    //public void SetColor(Color color) => _images.ForEach((i) => i.color = color);
    public void ResetAngle() => _thisTransform.localRotation = Quaternion.identity;
}
