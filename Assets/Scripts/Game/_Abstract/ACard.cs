using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class ACard : MonoBehaviour
{
    [SerializeField] protected float _scaleSizeShape = 0.85f;
    [Space]
    [SerializeField] protected CardBackground _cardBackground;
    [Space]
    [SerializeField] protected float _speedRotation = 90f;

    protected Transform _thisTransform;
    protected BoxCollider2D _collider;
    protected Vector3 _axis;
    protected Vector2 _currentSize;

    public bool ControlEnable { get; set; }
    public virtual bool IsInteractable { set => _collider.enabled = value; }
    public Vector3 LocalPosition { set => _thisTransform.localPosition = value; }

    protected void Awake()
    {
        _thisTransform = transform;
        _collider = GetComponent<BoxCollider2D>();
        IsInteractable = false;
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

    public virtual void SetSize(Vector2 size)
    {
        _currentSize = size;

        _collider.size = size;
        _cardBackground.SetSize(size);
    }

    public IEnumerator Turn90_Coroutine()
    {
        yield return _cardBackground.Rotation90Angle_Coroutine(-_axis, _speedRotation);
    }

    protected abstract void OnMouseDown();
        
}
