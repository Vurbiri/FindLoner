using System.Collections;
using UnityEngine;

public class BlockParticles : MonoBehaviour
{
    [SerializeField] private BlockParticleDigit _particleDigit;
    [SerializeField] private BlockParticleTrail _particleTrail;

    private Transform _thisTransform;
    private Vector3 _defaultPosition;

    private void Awake()
    {
        _thisTransform = transform;
        _defaultPosition = _thisTransform.localPosition;
    }

    public void Setup(BlockSettings settings)
    {
        _particleDigit.Setup(settings.MaterialParticle, settings.ColorBlock);
        _particleTrail.Setup(settings.ColorBlock);
    }

    public void StartMove()
    {
        _particleDigit.Stop();
        _particleTrail.Play();
    }
    public void StopMove()
    {
        _particleTrail.Stop();
        _particleDigit.Play();
    }

    public void ProjectionOn(Vector3 position, float alfaProjecting)
    {
        _thisTransform.position = position;
        _particleTrail.SetColorAlfa(alfaProjecting);
    }

    public void ProjectionOff()
    {
        _thisTransform.localPosition = _defaultPosition;
        _particleTrail.SetColorAlfa(1f);
    }
    
    public IEnumerator RemoveCoroutine() => _particleDigit.RemoveCoroutine();
    public IEnumerator ExplodeCoroutine() => _particleDigit.ExplodeCoroutine();

}
