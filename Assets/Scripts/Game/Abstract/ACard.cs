using System;
using System.Collections;
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
    protected bool _isInteractable = false;
    protected int _idGroup;
    protected Vector3 _axis;

    public int IdGroup => _idGroup;

    public event Action<ACard> EventSelected;

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
        EventSelected?.Invoke(this);
    }

    public abstract IEnumerator Show_Coroutine();
    public abstract IEnumerator Turn_Coroutine();

    
}
