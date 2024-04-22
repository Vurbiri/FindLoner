using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ACard : Graphic, IPointerDownHandler
{
    [Space]
    [SerializeField] protected CardBackground _cardBackground;
    [SerializeField] private float _pixelSizeDefault = 0.25f;
    [Space]
    [SerializeField] protected float _speedRotation = 90f;
    
    protected Transform _thisTransform;
    protected Vector3 _axis;

    protected override void Awake()
    {
        base.Awake();

        _thisTransform = transform;
        raycastTarget = false;
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

    protected void SetBackgroundPixelSize(float ratio) => _cardBackground.SetPixelSize(1 + _pixelSizeDefault * ratio);

    public IEnumerator Turn90_Coroutine()
    {
        yield return _cardBackground.Rotation90Angle_Coroutine(-_axis, _speedRotation);
    }
}
