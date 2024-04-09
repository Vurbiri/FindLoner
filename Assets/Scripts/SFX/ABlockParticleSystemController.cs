using UnityEngine;
using static UnityEngine.ParticleSystem;

public abstract class ABlockParticleSystemController : ParticleSystemController
{
    protected MainModule _mainModule;
    protected EmissionModule _emissionModule;
    protected ShapeModule _shapeModule;

    private float _rateOverTimeMultiplier;

    protected Color Color { set => _mainModule.startColor = value; }
    protected float EmissionTimeMultiplier { set => _emissionModule.rateOverTimeMultiplier = _rateOverTimeMultiplier * value; }

    protected override void Awake()
    {
        base.Awake();

        _mainModule = _thisParticle.main;
        _emissionModule = _thisParticle.emission;
        _shapeModule = _thisParticle.shape;

        _rateOverTimeMultiplier = _emissionModule.rateOverTimeMultiplier;
    }
}
