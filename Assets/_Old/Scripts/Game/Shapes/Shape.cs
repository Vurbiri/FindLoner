using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : AShape
{
    private DataGame _dataGame;
    private PoolBlocks _poolBlocks;
    private ArrayRandom<ShapeSettings> _randomSettings;
    private SoundSingleton _sound;
    private List<Block> _blocks;
    private ShapeSettings _currentShape = null;
    private ShapeSettings CurrentShape { set => IsSpawn = (_currentShape = value) != null; }
   
    public event Action<Shape> EventEndWork;

    public void Initialize(BlocksArea area, ArrayRandom<ShapeSettings> randomSettings, Bounds boundsGameArea, NextShape next)
    {
        Initialize(area, boundsGameArea);

        _dataGame = DataGame.Instance;
        _sound = SoundSingleton.InstanceF;
        _poolBlocks = PoolBlocks.InstanceF;
        _randomSettings = randomSettings;

        // Load
        if (next == null)
        {
            CurrentShape = null;
            return;
        }

        CurrentShape = _randomSettings[next.Id];
        _blocks = _poolBlocks.GetBlocks(_thisTransform, next.Blocks, _currentShape.BlocksPositions, _currentShape.Offset);
        _dragging.SetSize(_currentShape.Size);
    }

    public NextShape Save()
    {
        if(!IsSpawn)
            return null;

        return new(_currentShape.Type, _blocks);
    }

    public bool Spawn(bool isBomb)
    {
        ShapeSettings shape;
        int numberTries = isBomb ? 0 : _dataGame.Difficulty - _dataGame.CountShapesEmpty;
        bool isInsert;
        do
        {
            shape = _randomSettings.NextUnique;
            numberTries--;
            isInsert = _area.IsInsert(shape.Indexes);
        }
        while (!isInsert && !_area.IsFill && numberTries > 0);

        CurrentShape = shape;
        _blocks = _poolBlocks.GetBlocks(_thisTransform, _currentShape.BlocksPositions, _currentShape.Offset);
        _sound.PlaySpawn();
        _dragging.StartWork(_currentShape.Size);

        return isInsert;
    }

    public bool IsInsert()
    {
        if(!IsSpawn)
            return false;

        return _area.IsInsert(_currentShape.Indexes);
    }

    public void ResetState()
    {
        if (!IsSpawn)
            return;

        List<Block> blocksTemp = new(_blocks);
        _blocks.Clear();
        CurrentShape = null;

        blocksTemp.ForEach(block =>  StartCoroutine(block.RemoveSilentlyCoroutine()));
    }

    protected override void OnStartMoveShape()
    {
        _blocks.ForEach(block => block.StartMove());
        _isEnterShape = false;
    }

    protected override void OnMoveShape()
    {
        if (_area.IsEmptyCells(_blocks))
        {
            _blocks.ForEach(block => block.ProjectionOn());
            if (_oldIndexZeroBlock != _blocks[0].Index)
                _sound.PlayMove();

            _isEnterShape = true;
            _oldIndexZeroBlock = _blocks[0].Index;
            return;
        }

        if (_isEnterShape)
        {
            _isEnterShape = false;
            _oldIndexZeroBlock = Vector2Int.down;
            _sound.PlayMove();
            _blocks.ForEach(block => block.ProjectionOff());
        }
    }

    protected override void OnStopMoveShape()
    {
        _dragging.Enabled = false;

        if (_isEnterShape)
            Fixed();
        else
            StartCoroutine(ReturnToStartCoroutine());

        #region Local Functions
        void Fixed()
        {
            CurrentShape = null;
            _sound.PlayFixed();
            _dragging.ReturnToStart();

            _area.AddBlocks(_blocks);

            EventEndWork?.Invoke(this);
        }
        IEnumerator ReturnToStartCoroutine()
        {
            _sound.PlayToStart();
            yield return StartCoroutine(_dragging.ReturnToStartCoroutine(_timeToStart));
            _blocks.ForEach(block => block.StopMove());
            IsSpawn = true;
        }
        #endregion
    }
}
