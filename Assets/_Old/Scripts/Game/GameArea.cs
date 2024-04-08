using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private BlocksArea _area;
    [SerializeField] private Bomb _bomb;
    [SerializeField] private List<Shape> _shapes;
    [Space]
    [SerializeField] private AnimationController _animationController;
    [Space]
    [SerializeField] private ShapeSettings[] _shapeSettings;
    [Space]
    [SerializeField] private Bounds _bounds = new(new(8.925f, 7f), new(17.85f, 14f));
    [Space]
    [SerializeField] private float _timePauseBeforeSpawn = 0.4f;

    private DataGame _dataGame;
    private SoundSingleton _sound;
    private ShapeSettings _shapeOne;
    private int _countBonus;
    private ArrayRandom<ShapeSettings> _randomSettings;
    private readonly List<Shape> _shapesEmpty = new(NextShape.count);
    private bool _isBomb;
    private WaitForSeconds _pauseBeforeSpawn;
    private Coroutine _coroutineSpawn;

    public bool ControlEnable { 
        set 
        {
            _bomb.ControlEnable = value;
            _shapes.ForEach((shape) => shape.ControlEnable = value);
        } 
    }
    public bool IsOpen { get; private set; } = false;

    public event Action EventGameOver;
    public event Action EventChangingGame;

    private void Awake()
    {
        _sound = SoundSingleton.Instance;
        _dataGame = DataGame.Instance;
        _dataGame.EventChangeCountBombs += OnChangeCountBombs;
        _dataGame.EventNewStage += ReCalkWeightShapes;
        _countBonus = _dataGame.CountBonusWeight;

        _pauseBeforeSpawn = new(_timePauseBeforeSpawn);

        Array.Sort(_shapeSettings, (a, b) => a.Type.CompareTo(b.Type));
    }

    public void Initialize()
    {
        int i, bonus = _dataGame.BonusWeight, penalty = _dataGame.PenaltyWeightLast;
        (_shapeOne = _shapeSettings[0]).Initialize(bonus * 2);
        for (i = 1; i < _countBonus; i++)
            _shapeSettings[i].Initialize(bonus);
        for (; i < _shapeSettings.Length; i++)
            _shapeSettings[i].Initialize(penalty);
        _randomSettings = new(_shapeSettings);
        _shapeSettings = null;

        _bomb.Initialize(_area, _bounds);
        _bomb.EventExploding += SpawnBomb;

        _area.Load();

        Shape shape;
        for (i = 0; i < NextShape.count; i++)
        {
            (shape = _shapes[i]).Initialize(_area, _randomSettings, _bounds, _dataGame.NextShapes[i]);
            if (!shape.IsSpawn) 
                _shapesEmpty.Add(shape);
            shape.EventEndWork += OnEndWorkShape;
        }
    }

    public void Save()
    {
        for (int i = 0; i < NextShape.count; i++)
            _dataGame.NextShapes[i] = _shapes[i].Save();
    }

    public void Setup()
    {
        ReCalkWeightShapes();

        _isBomb = true;
        _shapesEmpty.Clear();
        _shapesEmpty.AddRange(_shapes.ToArray());
    }

    public void Open()
    {
        _animationController.PlayNormal();
        _sound.PlaySwitchWindows();
    }
    public void Close()
    {
        _animationController.PlayRevers();
        _sound.PlaySwitchWindows();
    }
   
    public IEnumerator StartWorkCoroutine()
    {
        yield return StartCoroutine(SpawnShapesCoroutine());
        yield return _pauseBeforeSpawn;
        SpawnBomb();
    }

    public void ResetState()
    {
        if (_coroutineSpawn != null)
        {
            StopCoroutine(_coroutineSpawn);
            _area.Blocking = false;
            _coroutineSpawn = null;
        }

        _bomb.ResetState();
        _shapes.ForEach((shape) => shape.ResetState());
        _area.RemoveAll();
        _sound.PlayReset();
    }

    private void ReCalkWeightShapes()
    {
        int i, bonus = _dataGame.BonusWeight, penalty = _dataGame.PenaltyWeightLast;
        _shapeOne.BonusWeight = bonus * 2;
        for (i = 1; i < _countBonus; i++)
            _randomSettings[i].BonusWeight = bonus;
        for (; i < _randomSettings.Count; i++)
            _randomSettings[i].BonusWeight = penalty;
        _randomSettings.ReInitialize();
    }

    private IEnumerator SpawnShapesCoroutine()
    {
        if (_area.IsFill)
        {
            EventGameOver?.Invoke();
            yield break;
        }

        bool isInsert = _isBomb || IsInsert();

        if (_shapesEmpty.Count < _dataGame.CountShapesEmpty)
        {
            if (!isInsert) EventGameOver?.Invoke();
            yield break;
        }

        _area.Blocking = true;
        foreach (var shape in _shapesEmpty)
        {
            yield return _pauseBeforeSpawn;
            isInsert = shape.Spawn(_isBomb) || isInsert;
        }
        _shapesEmpty.Clear();
        
        if (!isInsert) EventGameOver?.Invoke();

        _area.Blocking = false;
        _coroutineSpawn = null;

        #region Local Function
        bool IsInsert()
        {
            foreach (var shape in _shapes)
                if (shape.IsInsert())
                    return true;
            return false;
        }
        #endregion
    }

    private void SpawnBomb()
    {
        if (_bomb.IsSpawn || !(_isBomb = _dataGame.CountBombs > 0))
            return;
        
        _bomb.Spawn();
    }
    
    private void OnChangeCountBombs(int count)
    {
        if (_isBomb || count != 1)
            return;

        SpawnBomb();
    }

    private void OnEndWorkShape(Shape shape)
    {
        EventChangingGame?.Invoke();
        _shapesEmpty.Add(shape);
        _coroutineSpawn = StartCoroutine(SpawnShapesCoroutine());
    }

    private void OnDestroy()
    {
        if (DataGame.Instance == null)
            return;
       
        _dataGame.EventChangeCountBombs -= OnChangeCountBombs;
        _dataGame.EventNewStage -= ReCalkWeightShapes;
    }

    // События анимации
    public void EndOpen() => IsOpen = true;
    public void EndClose() => IsOpen = false;

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_bounds.center, _bounds.size);
    }

    private void OnValidate()
    {
        if (_shapeSettings == null || _shapeSettings.Length == 0)
            return;

        Array.Sort(_shapeSettings, (a, b) => a.Type.CompareTo(b.Type));
    }
#endif
}
