using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : ACard
{
    [SerializeField] private CardShape _cardShape;
    [Space]
    [SerializeField] private Color _colorNormal = Color.gray;
    [SerializeField] private Color _colorTrue = Color.gray;
    [SerializeField] private Color _colorError = Color.gray;

    public event Action<int> EventSelected;

    public IEnumerator Setup(Shape shape, int size, Vector3 axis, int idGroup)
    {
        _isInteractable = false;

        _idGroup = idGroup;
        _cardShape.SetShape(shape);
        _cardBackground.SetPixelSize(size);
        _cardBackground.SetColor(_colorNormal);

        _cardBackground.Rotation(axis, 90f);
        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(-axis, _speedRotation));

        _isInteractable = true;
    }

    public IEnumerator ReSetup(Shape shape, Vector3 axis, int idGroup)
    {
        _isInteractable = false;
        _idGroup = idGroup;

        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(axis, _speedRotation));
        yield return null;

        _cardShape.SetShape(shape);
        _cardShape.Mirror(axis);
        _cardBackground.SetColor(_colorNormal);

        yield return null;
        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(axis, _speedRotation));

        _isInteractable = true;
    }

    public void CheckCroup(int idGroup) 
    {
        _isInteractable = false;

        if (_idGroup != 0 && _idGroup != idGroup) 
            return;

        _cardBackground.SetColor(_idGroup == 0 ? _colorTrue : _colorError);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (_isInteractable)
            EventSelected?.Invoke(_idGroup);
    }
}
