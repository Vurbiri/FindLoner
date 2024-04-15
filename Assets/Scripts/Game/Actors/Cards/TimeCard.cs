using System.Collections;
using UnityEngine;

public class TimeCard : ACard<TimeCard>
{
    [Space]
    [SerializeField] private CardTimeShirt _cardShirt;
    [SerializeField] private CardTimeText _cardText;
    [SerializeField] private float _scaleFontSize = 0.45f;
    [Space]
    [SerializeField] private Color _colorBorderNormal = Color.gray;
    [SerializeField] private Color _colorBorderSelect = Color.gray;
    [SerializeField] private Color _colorBorderTrue = Color.gray;
    [SerializeField] private Color _colorBorderError = Color.gray;

    private bool _isShowShirt, _isDisable;

    public int Value => _value;
    public void InteractableOn() => _isInteractable = !_isDisable;
    public void InteractableOff() => _isInteractable = false;

    public void Setup(int value, float sizeCard, float count, Vector3 axis, bool isDisable = false)
    {
        _value = value;
        _axis = axis;
        _isInteractable = false;
        _isDisable = isDisable;

        _cardText.Setup(sizeCard * _scaleFontSize, value);
        SetBackgroundPixelSize(count);
        _cardBackground.SetColorBorder(_colorBorderNormal);

        _cardShirt.Mirror(axis);
        _cardShirt.SetActive(false);
        _cardText.SetActive(true);

        _cardBackground.Rotation(axis, 90f);
    }

    public override IEnumerator Show_Coroutine()
    {
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(-_axis, _speedRotation));
    }

    public IEnumerator TurnToShirt_Coroutine()
    {
        if (_isShowShirt || _isDisable) yield break;

        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));
        yield return null;

        _cardText.SetActive(false);
        _cardShirt.SetActive(true);
        
        yield return null;
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));

        _isShowShirt = true;
    }

    public IEnumerator TurnToValue_Coroutine()
    {
        if (!_isShowShirt) yield break;
        
        _isInteractable = false;

        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(-_axis, _speedRotation));
        yield return null;

        _cardShirt.SetActive(false);
        _cardText.SetActive(true);

        yield return null;
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(-_axis, _speedRotation));

        _isShowShirt = false;
    }

    public void Fixed()
    {
        _isDisable = true;
        _cardBackground.SetColorBorder(_colorBorderTrue);
    }

    public void SetColorError() => _cardBackground.SetColorBorder(_colorBorderError);

    public IEnumerator CardSelected_Coroutine()
    {
        _cardBackground.SetColorBorder(_colorBorderSelect);
        return TurnToValue_Coroutine();
    }

    public IEnumerator CardClose_Coroutine()
    {
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));
        yield return null;

        _cardText.SetActive(false);
        _cardShirt.SetActive(true);
        _cardBackground.SetColorBorder(_colorBorderNormal);

        yield return null;
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));

        _isShowShirt = true;
        _isInteractable = true;
    }

    public IEnumerator ReplaceCard_Coroutine(TimeCard targetCard, int value, float time)
    {
        _value = value;
        _cardText.SetText(_value);
        return _cardBackground.MoveTo_Coroutine(targetCard._cardBackground, time);
    }
    public IEnumerator ReplaceCard_Coroutine(TimeCard targetCard, float time) => ReplaceCard_Coroutine(targetCard, targetCard._value, time);
    public void ResetPosition() => _cardBackground.ResetPosition();

//#if UNITY_EDITOR
//    public static implicit operator string(TimeCard obj) => obj.ToString();
//    public override string ToString() => Value.ToString();
//#endif
}
