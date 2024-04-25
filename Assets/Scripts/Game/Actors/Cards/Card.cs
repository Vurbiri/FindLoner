using System;
using System.Collections;
using UnityEngine;

public class Card : ACard
{
    [Space]
    [SerializeField] private CardShape _cardShape;
    [Space]
    [SerializeField] private Color _colorBorderNormal = Color.gray;
    [SerializeField] private Color _colorBorderTrue = Color.gray;
    [SerializeField] private Color _colorBorderError = Color.gray;

    public int IdGroup => _idGroup;
    private int _idGroup;
    private Shape _shape;

    public static bool IsCheat = false;

    public event Action<Card> EventSelected;

    public void Setup(Shape shape, Vector2 axis, int idGroup)
    {
        ReSetup(shape, axis, idGroup);

        _cardShape.SetShape(shape);
        _cardBackground.SetColorBorder(IsCheat && idGroup == 0 ? Color.white : _colorBorderNormal);

        _cardShape.ResetAngle();
        _cardBackground.Set90Angle(axis);
    }

    public void ReSetup(Shape shape, Vector3 axis, int idGroup)
    {
        IsInteractable = false;
        ControlEnable = true;

        _idGroup = idGroup;
        _axis = axis;
        _shape = shape;
    }

    public override void SetSize(Vector2 size)
    {
        if (_currentSize == size)
            return;

        base.SetSize(size);
        _cardShape.SetSize(size * _scaleSizeShape);
    }

    public IEnumerator Turn_Coroutine()
    {
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));
        yield return null;

        _cardShape.SetShape(_shape);
        _cardShape.Mirror(_axis);
        _cardBackground.SetColorBorder(IsCheat && _idGroup == 0 ? Color.white : _colorBorderNormal);

        yield return null;
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));
    }

    public void CheckCroup(int idGroup) 
    {
        IsInteractable = false;

        if (_idGroup != 0 && _idGroup != idGroup) 
            return;

        _cardBackground.SetColorBorder(_idGroup == 0 ? _colorBorderTrue : _colorBorderError);
    }

    protected override void OnMouseDown()
    {
        if(!ControlEnable) return;
        
        IsInteractable = false;
        EventSelected?.Invoke(this);
    }
}
