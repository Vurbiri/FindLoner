using System.Collections;
using UnityEngine;

public class CardBackground : ACardComponent
{
    [SerializeField] private SpriteRenderer _border;

    public IEnumerator Rotation90Angle_Coroutine(Vector3 axis, float speed)
    {
        float angle = 0, rotation;
        Quaternion target = _thisTransform.rotation * Quaternion.Euler(axis * 90f);

        while (angle < 90f) 
        {
            yield return null;

            rotation = speed * Time.deltaTime;
            _thisTransform.rotation *= Quaternion.Euler(axis * rotation);
            angle += rotation;
        }

        _thisTransform.rotation = target;
    }

    public IEnumerator MoveTo_Coroutine(CardBackground targetBackground, float time)
    {
        Vector3 current = _thisTransform.position, target = targetBackground._thisTransform.position;
        Vector3 speed = (target - current) / time;

        while (time > 0)
        {
            yield return null;
            _thisTransform.position = current += speed * Time.deltaTime;
            time -= Time.deltaTime;
        }

        _thisTransform.position = target;
        yield return null;
    }

    public void ResetPosition() => _thisTransform.localPosition = Vector3.zero;

    public void SetColorBorder(Color color) => _border.color = color;

    public override void SetSize(Vector2 size)
    {
        _thisSprite.size =  size;
        _border.size = size;
    }

    public void SetOrderInLayer(Increment layers)
    {
        _thisSprite.sortingOrder = layers.Next;
        _border.sortingOrder = layers.Next;
    }
}
