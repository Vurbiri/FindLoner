using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Bomb : AShape, IBlock
{
    [SerializeField] private BombSFX _bombSFX;

    private DataGame _dataGame;

    public Vector2Int Index { get; private set; }
    public Vector3 Position => _thisTransform.position;

    private Vector3 _floatIndex;
    private readonly Vector3 _size = new(1f, 1f, 0);
    private readonly Vector3 _offsetIndexProjection = new(0.5f, 0.5f, 0);

    public event Action EventExploding;

    public new void Initialize(BlocksArea area, Bounds boundsGameArea)
    {
        base.Initialize(area, boundsGameArea);

        _dataGame = DataGame.Instance;

        _bombSFX.Initialize(area);
        _dragging.SetSize(_size);

        IsSpawn = false;
    }

    public void Spawn()
    {
        _bombSFX.Spawn();
        _dragging.Enabled = true;
        IsSpawn = true;
    }

    public void ResetState()
    {
        if (!IsSpawn)
            return;

        _area.Blocking = false;
        IsSpawn = false;
        _dragging.ReturnToStart();
        _bombSFX.ResetState();
    }

    public Vector2Int GetIndex()
    {
        return Index = (_floatIndex = _thisTransform.position.Floor()).RoundToInt();
    }

    protected override void OnStartMoveShape()
    {
        _isEnterShape = false;
        _dataGame.CountBombs--;
    }

    protected override void OnMoveShape()
    {
        if (_area.IsEmptyCell(this))
        {
            _isEnterShape = true;
            _bombSFX.ProjectionOn(_floatIndex + _offsetIndexProjection, Index, _oldIndexZeroBlock != Index);
            _oldIndexZeroBlock = Index;
            return;
        }

        if (_isEnterShape)
        {
            _isEnterShape = false;
            _oldIndexZeroBlock = Vector2Int.down;
            _bombSFX.ProjectionOff();
        }
    }

    protected override void OnStopMoveShape()
    {
        _dragging.Enabled = false;

        if (_isEnterShape)
            StartCoroutine(ExplodeCoroutine());
        else
            StartCoroutine(ReturnToStartCoroutine());

        #region Local Functions
        IEnumerator ExplodeCoroutine()
        {
            _area.Blocking = true;
            IsSpawn = false;

            _area.ExplodeBlocks(Index);
            yield return StartCoroutine(_bombSFX.ExplodeCoroutine());
            
            _area.Blocking = false;
            _dragging.ReturnToStart();
            EventExploding?.Invoke();
        }
        IEnumerator ReturnToStartCoroutine()
        {
            _bombSFX.MoveToStart();
            yield return StartCoroutine(_dragging.ReturnToStartCoroutine(_timeToStart));
            _dataGame.CountBombs++;
            IsSpawn = true;
        }
        #endregion
    }
}
