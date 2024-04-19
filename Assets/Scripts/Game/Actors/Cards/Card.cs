using System.Collections;
using UnityEngine;

public class Card : ACard<Card>
{
    [Space]
    [SerializeField] private CardShape _cardShape;
    [Space]
    [SerializeField] private Color _colorBorderNormal = Color.gray;
    [SerializeField] private Color _colorBorderTrue = Color.gray;
    [SerializeField] private Color _colorBorderError = Color.gray;

    private Shape _shape;

    public int IdGroup => _idGroup;
    private int _idGroup;

    private bool _isCheats;

    public bool IsInteractable { set => _isInteractable = value; }

    public void Setup(Shape shape, int size, Vector3 axis, int idGroup, bool isCheats)
    {
        _isInteractable = false;

        isCheats = isCheats && idGroup == 0;
        _idGroup = idGroup;
        _axis = axis;

        _cardShape.SetShape(shape);
        SetBackgroundPixelSize(size);
        _cardBackground.SetColorBorder(isCheats ? Color.white : _colorBorderNormal);

        _cardShape.ResetAngle();
        _cardBackground.Set90Angle(axis);
    }

    public void ReSetup(Shape shape, Vector3 axis, int idGroup, bool isCheats)
    {
        _isInteractable = false;

        _isCheats = isCheats && idGroup == 0;
        _idGroup = idGroup;
        _axis = axis;
        _shape = shape;
    }

    public IEnumerator Turn_Coroutine()
    {
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));
        yield return null;

        _cardShape.SetShape(_shape);
        _cardShape.Mirror(_axis);
        _cardBackground.SetColorBorder(_isCheats ? Color.white : _colorBorderNormal);

        yield return null;
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));

        _isInteractable = true;
    }

    public void CheckCroup(int idGroup) 
    {
        _isInteractable = false;

        if (_idGroup != 0 && _idGroup != idGroup) 
            return;

        _cardBackground.SetColorBorder(_idGroup == 0 ? _colorBorderTrue : _colorBorderError);
    }
}
