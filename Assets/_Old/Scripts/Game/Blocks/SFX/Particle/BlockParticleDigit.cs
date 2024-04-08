using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlockParticleDigit : ABlockParticleSystemController
{
    #region SerializeField
    [SerializeField, Foldout("Remove")] private float _timeRemoveStart = 1f;
    [SerializeField, Foldout("Remove")] private float _timeRemoveNext = 0.3f;
    [Space]
    [SerializeField, Foldout("Remove")] private float _radialSpeedRemove = 2f;
    [SerializeField, Foldout("Remove")] private float _sizeRemove = 1.4f;
    [SerializeField, Foldout("Remove")] private float _gravityRemove = 1.4f;
    [Space]
    [SerializeField, Foldout("Remove")] private float _speedRemoveStart = 0f;
    [SerializeField, Foldout("Remove")] private float _speedRemoveMiddle = 2f;
    [Space]
    [SerializeField, Foldout("Remove")] private float _emissionRemoveStart = 15f;
    [SerializeField, Foldout("Remove")] private float _emissionRemoveMiddle = 10f;
    [SerializeField, Foldout("Remove")] private float _emissionRemoveEnd = 5f;

    [SerializeField, Foldout("Explode")] private float _timeExplodeStart = 0.65f;
    [SerializeField, Foldout("Explode")] private float _timeExplodeNext = 0.2f;
    [Space]
    [SerializeField, Foldout("Explode")] private float _speedExplodeStart = 0f;
    [SerializeField, Foldout("Explode")] private float _speedExplodeMiddle = 4f;
    [Space]
    [SerializeField, Foldout("Explode")] private float _radialSpeedExplodeStart = 2f;
    [SerializeField, Foldout("Explode")] private float _radialSpeedExplodeMiddle = -3f;
    [Space]
    [SerializeField, Foldout("Explode")] private float _emissionExplodeStart = 14f;
    [SerializeField, Foldout("Explode")] private float _emissionExplodeMiddle = 8f;
    [SerializeField, Foldout("Explode")] private float _emissionExplodeEnd = 4f;
    #endregion
        
    private VelocityOverLifetimeModule _velocityOverLifetimeModule;
    private ParticleSystemRenderer _particleRenderer;
        
    private float _radialMultiplier;
    private MinMax _sizeMultiplier;
    private MinMaxVector3 _speedLinear;
        
    private float SizeMultiplier { set => _mainModule.startSize = _sizeMultiplier.GetValueMultiplier(value); }
    private float RadialSpeedMultiplier { set => _velocityOverLifetimeModule.radialMultiplier = _radialMultiplier * value; }
    private float SpeedLinearMultiplier 
    {
        set
        {
            _speedLinear.Multiplier = value;
            _velocityOverLifetimeModule.SetLinearFromMinMaxVector3(_speedLinear);
        }
    }

    private WaitForSeconds _waitRemoveStart;
    private WaitForSeconds _waitRemoveNext;
    private WaitForSeconds _waitExplodeStart;
    private WaitForSeconds _waitExplodeNext;

    protected override void Awake()
    {
        base.Awake();
                
        _velocityOverLifetimeModule = _thisParticle.velocityOverLifetime;
        _particleRenderer = GetComponent<ParticleSystemRenderer>();
                
        _radialMultiplier = _velocityOverLifetimeModule.radialMultiplier;
        _sizeMultiplier = _mainModule.startSize;
        _speedLinear = _velocityOverLifetimeModule.LinearToMinMaxVector3();

        _waitRemoveStart = new(_timeRemoveStart);
        _waitRemoveNext = new(_timeRemoveNext);
        _waitExplodeStart = new(_timeExplodeStart);
        _waitExplodeNext = new(_timeExplodeNext);
    }

    public void Setup(Material material, Color color)
    {
        _shapeModule.radiusThickness = 1f;
        EmissionTimeMultiplier = 1f;
        _mainModule.gravityModifier = 0;
        RadialSpeedMultiplier = 1f;
        SpeedLinearMultiplier = 1f;
        SizeMultiplier = 1f;
        Color = color;
        _particleRenderer.sharedMaterial = material;
        Clear();
        Play();
    }

    public IEnumerator RemoveCoroutine()
    {
        EmissionTimeMultiplier = _emissionRemoveStart;
        RadialSpeedMultiplier = _radialSpeedRemove;
        SpeedLinearMultiplier = _speedRemoveStart;
        SizeMultiplier = _sizeRemove;

        yield return _waitRemoveStart;

        EmissionTimeMultiplier = _emissionRemoveMiddle;
        SpeedLinearMultiplier = _speedRemoveMiddle;
        _mainModule.gravityModifier = _gravityRemove;

        yield return _waitRemoveNext;

        EmissionTimeMultiplier = _emissionRemoveEnd;

        yield return _waitRemoveNext;
    }


    public IEnumerator ExplodeCoroutine()
    {
        EmissionTimeMultiplier = _emissionExplodeStart;
        RadialSpeedMultiplier = _radialSpeedExplodeStart;
        SpeedLinearMultiplier = _speedExplodeStart;

        yield return _waitExplodeStart;

        EmissionTimeMultiplier = _emissionExplodeMiddle;
        SpeedLinearMultiplier = _speedExplodeMiddle;
        RadialSpeedMultiplier = _radialSpeedExplodeMiddle;

        yield return _waitExplodeNext;

        EmissionTimeMultiplier = _emissionExplodeEnd;

        yield return _waitExplodeNext;
    }
}
