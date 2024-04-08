using System.Collections;
using UnityEngine;

public class BombSprites : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRendererBomb;
    [SerializeField] private SpriteRenderer _spriteRendererNumber;
    [Space]
    [SerializeField] private Vector2 _sizeMinMaxTick = new(0.7f, 1.1f);

    private float _speedTick;
    private Coroutine _coroutineTick = null;

    private Transform _thisTransform;
    private Vector3 _defaultPosition;

    public void Initialize(float timeTick)
    {
        _thisTransform = transform;
        _defaultPosition = _thisTransform.localPosition;

        _speedTick = (_sizeMinMaxTick.y - _sizeMinMaxTick.x) / timeTick;
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void ProjectionOn(Vector3 position)
    {
        _thisTransform.position = position;
        _coroutineTick ??= StartCoroutine(TickBombCoroutine());
    }

    public void ProjectionOff()
    {
        _thisTransform.localPosition = _defaultPosition;
        if (_coroutineTick != null)
        {
            StopCoroutine(_coroutineTick);
            _coroutineTick = null;
            _spriteRendererNumber.size = Vector2.one;
        }
    }

    public void Explode()
    {
        ProjectionOff();
        gameObject.SetActive(false);
    }

    public void ResetState() => Explode();

    private IEnumerator TickBombCoroutine()
    {
        float ratio = _sizeMinMaxTick.y;
        Vector2 size = Vector2.one;
        while (true) 
        {
            _spriteRendererNumber.size = size * ratio;
            yield return null;
            if(ratio < _sizeMinMaxTick.x)
                ratio = _sizeMinMaxTick.y;
            else
                ratio -= _speedTick * Time.deltaTime;
        }
    }
}
