using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BlockSFX)), SelectionBase]
public class Block : APooledObject<Block>, IBlock
{
    [SerializeField] private BlockSFX _blockSFX;

    public Vector2Int Index {  get; set; }
    public Vector3 Position => _thisTransform.position;
    public int Digit { get; private set; }
    public bool IsOne { get; private set; }

    private Vector3 _floatIndex;

    private readonly Vector3 _offsetIndexProjection = new(0.5f, 0.5f, 0);
    private const string NAME = "Block_{0}";

    public override void Initialize()
    {
        _blockSFX.Initialize();
        base.Initialize();
    }

    public void Setup(Vector3 position, BlockSettings settings)
    {
        TypeSetup(settings);
        _thisTransform.localPosition = position;
        Activate();
    }

    public void TypeSetup(BlockSettings settings)
    {
        Digit = settings.Digit;
        IsOne = Digit == 1;

        gameObject.name = string.Format(NAME, Digit);
        _blockSFX.Setup(settings);
    }

    public void StartMove() => _blockSFX.StartMove();
    public void StopMove() => _blockSFX.StopMove();

    public Vector2Int GetIndex()
    {
        return Index = (_floatIndex = _thisTransform.position.Floor()).RoundToInt();
    }

    public void ProjectionOn()
    {
        _blockSFX.ProjectionOn(_floatIndex + _offsetIndexProjection);
    }
    public void ProjectionOff()
    {
        _blockSFX.ProjectionOff();
    }
    public void Fixed(Transform parent)
    {
        SetParent(parent);
        _thisTransform.localPosition = _floatIndex;
        _blockSFX.Fixed();
    }

    public bool IsEqualDigit(Block other)
    {
        if (other == null) return false;
        if (this == other) return false;
        return Digit == other.Digit;
    }

    public IEnumerator RemoveCoroutine()
    {
        yield return _blockSFX.RemoveCoroutine();
        Deactivate();
    }

    public IEnumerator RemoveSilentlyCoroutine()
    {
        yield return _blockSFX.RemoveSilentlyCoroutine();
        Deactivate();
    }

    public IEnumerator ExplodeCoroutine()
    {
        yield return _blockSFX.ExplodeCoroutine();
        Deactivate();
    }

    //public override void Deactivate()
    //{
    //    //StopAllCoroutines();
    //    base.Deactivate();
    //}
}


