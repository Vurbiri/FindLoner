using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimeCard : ACard
{
    [SerializeField] private CardTimeShirt _cardShirt;
    [SerializeField] private CardTimeText _cardText;
    [Space]
    [SerializeField] private Color _colorBorderNormal = Color.gray;
    [SerializeField] private Color _colorBorderSelect = Color.gray;

    public bool _isShowShirt;

    public int Value { get; private set; }
    public bool IsInteractable { set => _isInteractable = value; }

    public void Setup(int value, float sizeCard, float count, Vector3 axis, int idGroup)
    {
        _isInteractable = false;

        _idGroup = idGroup;
        Value = value;
        _axis = axis;
        
        _cardText.Setup(sizeCard, value);
        _cardBackground.SetPixelSize(count);
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

    public override IEnumerator Turn_Coroutine()
    {
        if (_isShowShirt) yield break;

        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));
        yield return null;

        _cardText.SetActive(false);
        _cardShirt.SetActive(true);
        
        yield return null;
        yield return StartCoroutine(_cardBackground.Rotation90Angle_Coroutine(_axis, _speedRotation));

        _isInteractable = _isShowShirt = true;
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

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        _isInteractable = false;
        _cardBackground.SetColorBorder(_colorBorderSelect);
        base.OnPointerDown(eventData);
    }

#if UNITY_EDITOR
    public static implicit operator string(TimeCard obj) => obj.ToString();
    public override string ToString() => Value.ToString();
#endif
}
