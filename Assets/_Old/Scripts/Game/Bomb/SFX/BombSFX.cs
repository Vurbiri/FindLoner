using System.Collections;
using UnityEngine;

public class BombSFX : MonoBehaviour
{
    [SerializeField] private BombSprites _sprites;
    [SerializeField] private BombAffectedArea _affectedArea;
    [SerializeField] private BombParticles _particles;
    [Space]
    [SerializeField] private float _timeTick = 0.5f;

    private SoundSingleton _sound;
    private bool _isProjection;

    public void Initialize(BlocksArea area)
    {
        _sound = SoundSingleton.Instance;
        _sprites.Initialize(_timeTick);
        _affectedArea.Initialize(area, _timeTick);
        _particles.Initialize();
    }

    public void Spawn()
    {
        _sprites.Spawn();
        _particles.Spawn();
        _isProjection = false;
        _sound.PlaySpawnBomb();
    }

    public void MoveToStart() => _sound.PlayToStart();

    public void ProjectionOn(Vector3 position, Vector2Int index, bool isShift)
    {
        _sprites.ProjectionOn(position);
        _affectedArea.ProjectionOn(position, index, isShift);
        _particles.ProjectionOn(position);
        if(!_isProjection)
        {
            _isProjection = true;
            _sound.StartTickingBomb();
        }
    }

    public void ProjectionOff()
    {
        _sprites.ProjectionOff();
        _affectedArea.ProjectionOff();
        _particles.ProjectionOff();
        _isProjection = false;
        _sound.Stop();
    }

    public IEnumerator ExplodeCoroutine()
    {
        _sprites.Explode();
        _affectedArea.Explode();
        _isProjection = false;
        _sound.Stop();
        _sound.PlayExplode();
        return _particles.ExplodeCoroutine();
    }

    public void ResetState()
    {
        _isProjection = false;
        _sound.Stop();
        _sprites.ResetState();
        _affectedArea.ResetState();
        _particles.ResetState();
    }
}
