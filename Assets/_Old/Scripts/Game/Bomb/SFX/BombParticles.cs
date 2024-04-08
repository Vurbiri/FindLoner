using System.Collections;
using UnityEngine;

public class BombParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystemController _particleSparks;
    [SerializeField] private ParticleSystemController _particleExplode;

    private Transform _thisTransform;
    private Vector3 _defaultPosition;

    private WaitWhile _waitPlayExplode;

    public void Initialize()
    {
        _thisTransform = transform;
        _defaultPosition = _thisTransform.localPosition;

        _waitPlayExplode = new(()=> _particleExplode.IsPlaying);

        _particleSparks.ClearAndStop();
        _particleExplode.ClearAndStop();
    }

    public void Spawn()
    {
        _particleSparks.Play();
    }

    public void ProjectionOn(Vector3 position)
    {
        _thisTransform.position = position;
        _particleSparks.Stop();
    }

    public void ProjectionOff()
    {
        _thisTransform.localPosition = _defaultPosition;
        _particleSparks.Play();
    }

    public IEnumerator ExplodeCoroutine()
    {
        _particleExplode.Play();
        yield return _waitPlayExplode;
        _thisTransform.localPosition = _defaultPosition;
    }

    public void ResetState()
    {
        _thisTransform.localPosition = _defaultPosition;
        _particleSparks.Stop();
        _particleExplode.Play();
    }
}
