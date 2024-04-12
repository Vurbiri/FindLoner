using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimeCard : ACard
{
    [SerializeField] private CardTimeShirt _cardShirt;
    [SerializeField] private CardTimeText _cardText;

    private Vector3 _axis;

    public event Action<TimeCard> EventSelected;

    public IEnumerator Setup(int value, float sizeCard, float count, Vector3 axis, int idGroup)
    {
        _isInteractable = false;

        _idGroup = idGroup;
        _axis = axis;

        _cardText.Setup(sizeCard, value);
        _cardBackground.SetPixelSize(count);

        _cardShirt.Mirror(axis);
        _cardShirt.SetActive(false);
        _cardText.SetActive(true);

        _cardBackground.Rotation(axis, 90f);
        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(-axis, _speedRotation));
    }

    public IEnumerator Hide()
    {
        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(_axis, _speedRotation));
        yield return null;

        _cardText.SetActive(false);
        _cardShirt.SetActive(true);
        
        yield return null;
        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(_axis, _speedRotation));

        _isInteractable = true;
    }

    public IEnumerator Show()
    {
        _isInteractable = false;

        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(-_axis, _speedRotation));
        yield return null;

        _cardShirt.SetActive(false);
        _cardText.SetActive(true);

        yield return null;
        yield return StartCoroutine(_cardBackground.Rotation90AngleCoroutine(-_axis, _speedRotation));
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        _isInteractable = false;
        EventSelected?.Invoke(this);
    }
}
