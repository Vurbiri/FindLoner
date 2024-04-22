using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimeCard : ACard
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

    private bool _isShowShirt, _isFixed;
    private BonusTime _bonus;

    private Action<TimeCard> actionSelected;

    public bool IsNotZero => !_isFixed && _bonus.Value != 0;
    public int Value => _bonus.Value;
    public BonusTime Bonus => _bonus;
    public override bool raycastTarget { get => base.raycastTarget; set => base.raycastTarget = value && !_isFixed; }
    
    public void Setup(BonusTime bonus, float sizeCard, float count, Vector3 axis, Action<TimeCard> action, bool isFixed = false)
    {
        _bonus = bonus;
        _axis = axis;
        base.raycastTarget = false;
        _isFixed = isFixed;

        _cardText.Setup(sizeCard * _scaleFontSize, bonus);
        SetBackgroundPixelSize(count);
        _cardBackground.SetColorBorder(_colorBorderNormal);

        _cardShirt.Set180Angle(axis);
        _cardText.ResetAngle();

        _cardShirt.SetActive(false);
        _cardText.SetActive(true);

        _cardBackground.Set90Angle(axis);

        actionSelected = action;
    }
    
    public IEnumerator TurnToShirt_Coroutine()
    {
        if (_isShowShirt || _isFixed) yield break;

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

        base.raycastTarget = false;

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
        _isFixed = true;
        base.raycastTarget = false;
        _cardBackground.SetColorBorder(_colorBorderTrue);
    }

    public void SetColorError() => _cardBackground.SetColorBorder(_colorBorderError);

    public IEnumerator CardSelected_Coroutine()
    {
        _cardBackground.SetColorBorder(_colorBorderSelect);
        return TurnToValue_Coroutine();
    }

    public IEnumerator CardHideAndUnsubscribe_Coroutine()
    {
        actionSelected = null;
        yield return _cardBackground.Rotation90Angle_Coroutine(-_axis, _speedRotation);
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
    }

    public IEnumerator ReplaceCard_Coroutine(TimeCard targetCard, BonusTime bonus, float time)
    {
        targetCard.SetBonus(bonus);
        return _cardBackground.MoveTo_Coroutine(targetCard._cardBackground, time);
    }
    public IEnumerator ReplaceCard_Coroutine(TimeCard targetCard, float time) => ReplaceCard_Coroutine(targetCard, _bonus, time);
    public void ResetPosition() => _cardBackground.ResetPosition();
    private void SetBonus(BonusTime bonus)
    {
        _bonus = bonus;
        _cardText.ReSetup(bonus);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.raycastTarget = false;
        actionSelected?.Invoke(this);
    }
}
