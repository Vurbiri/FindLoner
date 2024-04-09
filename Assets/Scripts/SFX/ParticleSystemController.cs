using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    protected ParticleSystem _thisParticle;
    
    protected virtual void Awake()
    {
        _thisParticle = GetComponent<ParticleSystem>();
    }

    public bool IsPlaying => _thisParticle.isPlaying;

    public void Play() => _thisParticle.Play();
    public void Stop() => _thisParticle.Stop();
    public void Clear() => _thisParticle.Clear();

    public void ClearAndStop()
    {
        _thisParticle.Clear();
        _thisParticle.Stop();
    }
}
