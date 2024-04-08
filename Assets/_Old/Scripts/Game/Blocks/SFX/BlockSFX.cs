using System.Collections;
using UnityEngine;

public class BlockSFX : MonoBehaviour
{
    [SerializeField] private BlockParticles _particles;
    [SerializeField] private BlockSprites _sprites;
    [Space]
    [SerializeField, Range(0f, 1f)] private float _alfaProjecting = 0.5f;

    private SoundSingleton _sound;

    public void Initialize()
    {
        _sound = SoundSingleton.InstanceF;
    }

    public void Setup(BlockSettings settings)
    {
        _sprites.Setup(settings);
        _particles.Setup(settings);
    }

    public void StartMove()
    {
        _particles.StartMove();
        _sprites.StartMove();
    }

    public void StopMove()
    {
        _particles.StopMove();
        _sprites.StopMove();
    }

    public void ProjectionOn(Vector3 position)
    {
        _sprites.ProjectionOn(position, _alfaProjecting);
        _particles.ProjectionOn(position, _alfaProjecting);
    }
    public void ProjectionOff()
    {
        _sprites.ProjectionOff();
        _particles.ProjectionOff();
    }

    public void Fixed()
    {
        ProjectionOff();
        StopMove();
    }

    public IEnumerator RemoveCoroutine()
    {
        _sound.PlayRemove();
        _sprites.Off();
        return _particles.RemoveCoroutine();
    }
    public IEnumerator RemoveSilentlyCoroutine()
    { 
        _sprites.Off();
        return _particles.RemoveCoroutine();
    }

    public IEnumerator ExplodeCoroutine()
    {
        _sprites.Off();
        return _particles.ExplodeCoroutine();
    }
}
