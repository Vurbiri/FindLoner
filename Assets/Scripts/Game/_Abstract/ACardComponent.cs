using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class ACardComponent : MonoBehaviour
{
    protected SpriteRenderer _thisSprite;
    protected Transform _thisTransform;

    protected virtual void Awake()
    {
        _thisSprite = GetComponent<SpriteRenderer>();
        _thisTransform = transform;
    }

    public virtual void SetSize(Vector2 size)
    {
        _thisSprite.size = size;
    }
    
    public void Mirror(Vector2 axis)
    {
        _thisSprite.flipX = axis.x > 0;
        _thisSprite.flipY = axis.y > 0;
    }
    public void Set90Angle(Vector3 axis) => _thisTransform.localRotation = Quaternion.Euler(axis * 90);
    public void Rotation(Vector3 axis, float angle) => _thisTransform.rotation *= Quaternion.Euler(axis * angle);
}
