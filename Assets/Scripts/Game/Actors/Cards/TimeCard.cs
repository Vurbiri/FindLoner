using System;
using System.Collections;
using UnityEngine;

public class TimeCard : ACard
{
    [Space]
    [SerializeField] private CardTimeShirt _cardShirt;
    [SerializeField] private CardTimeText _cardText;
    
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
    public override bool IsInteractable { set => base.IsInteractable = value && !_isFixed; }
    
    public void Setup(BonusTime bonus, Vector2 axis, Action<TimeCard> action, bool isFixed = false)
    {
        base.IsInteractable = false;
        ControlEnable = true;

        _bonus = bonus;
        _axis = axis;
        _isFixed = isFixed;

        _cardText.Setup(bonus);

        _cardBackground.SetColorBorder(_colorBorderNormal);

        _cardShirt.Set180Angle(axis);
        _cardText.ResetAngle();

        _cardShirt.SetActive(false);
        _cardText.SetActive(true);

        _cardBackground.Set90Angle(axis);

        actionSelected = action;
    }

    public override void SetSize(Vector2 size)
    {
        if (_currentSize == size)
            return;

        base.SetSize(size);
        _cardShirt.SetSize(size * _scaleSizeShape);
        _cardText.SetSize(size);
    }

    public void SetOrderInLayer(Increment layers)
    {
        _cardBackground.SetOrderInLayer(layers);
        _cardShirt.SetOrderInLayer(layers.Current);
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

        base.IsInteractable = false;

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
        base.IsInteractable = false;
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

    protected override void OnMouseDown()
    {
        if (!ControlEnable) return;

        base.IsInteractable = false;
        actionSelected?.Invoke(this);
    }
}
