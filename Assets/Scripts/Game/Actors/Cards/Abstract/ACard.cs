using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ACard : Graphic, IPointerDownHandler
{
    [Space]
    [SerializeField] protected float _speedRotation = 90f;
    [Space]
    [SerializeField] protected CardBackground _cardBackground;

    protected Transform _thisTransform;

    protected int _idGroup;
    protected bool _isInteractable = false;

    protected override void Awake()
    {
        base.Awake();

        _thisTransform = transform;
    }

    public void Activate(Transform parent)
    {
        _thisTransform.SetParent(parent);
        gameObject.SetActive(true);
    }

    public virtual void Deactivate(Transform parent)
    {
        gameObject.SetActive(false);
        _thisTransform.SetParent(parent);
    }

    public abstract void OnPointerDown(PointerEventData eventData);
}
