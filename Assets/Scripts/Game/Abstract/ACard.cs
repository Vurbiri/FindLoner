using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ACard<T> : Graphic, IPointerDownHandler where T : ACard<T>
{
    [Space]
    [SerializeField] protected CardBackground _cardBackground;
    [SerializeField] private float _pixelSizeDefault = 0.25f;
    [Space]
    [SerializeField] protected float _speedRotation = 90f;
    

    protected Transform _thisTransform;
    protected bool _isInteractable = false;
    protected int _value;
    protected Vector3 _axis;

    public event Action<T> EventSelected;

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

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        _isInteractable = false;
        EventSelected?.Invoke((T)this);
    }

    protected void SetBackgroundPixelSize(float ratio) => _cardBackground.SetPixelSize(1 + _pixelSizeDefault * ratio);

    public abstract IEnumerator Show_Coroutine();
}
