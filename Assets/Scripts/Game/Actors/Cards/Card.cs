using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : Graphic, IPointerDownHandler
{
    [Space]
    [SerializeField] private CardBackground _cardBackground;
    [SerializeField] private CardShape _cardShape;
    [Space]
    [SerializeField] private float _speedRotation = 90f;
    [Space]
    [SerializeField] private Color _colorNormal = Color.gray;
    [SerializeField] private Color _colorTrue = Color.gray;
    [SerializeField] private Color _colorError = Color.gray;
    [SerializeField] private float _fadeDuration = 0.2f;


    public void Setup(Shape shape, float size, Vector3 axis)
    {
        _cardShape.SetShape(shape);
        _cardBackground.SetPixelSize(size);
        _cardBackground.SetColor(_colorNormal);
        _cardBackground.Rotation(axis, 90f);

        StartCoroutine(_cardBackground.Rotation90AngleCoroutine(-axis, _speedRotation));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(RotationCoroutine(Vector3.left));
    }

    private IEnumerator RotationCoroutine(Vector3 axis)
    {
        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(axis, _speedRotation));
        _cardShape.Mirror(axis);
        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(axis, _speedRotation));
    }
}
