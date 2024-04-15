using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

[RequireComponent(typeof(BonusLevelSingle), typeof(BonusLevelPair))]
[RequireComponent(typeof(TimeCardsArea), typeof(GridLayoutGroup))]
public class BonusLevels : MonoBehaviour
{
    [SerializeField] private float _saturationMin = 0.275f;
    [SerializeField] private float _brightnessMin = 0.25f;
    [Space]
    [SerializeField] private float _timeShowEndLevel = 1.75f;
    [Space]
    [SerializeField] private float _delayOpenPerAll = 1.5f;
    [SerializeField] private float _delayTurnPerAll = 2.5f;
#if UNITY_EDITOR
    [SerializeField] private BonusLevelTypes _type;
#endif

    private BonusLevelSingle _levelSingle;
    private BonusLevelPair _levelPair;

    private TimeCardsArea _cardsArea;
    private GridLayoutGroup _thisGrid;
    private Vector2 _sizeArea, _defaultSpacing;

    public event Action<int> EventAddTime { add { _levelSingle.EventSelectedCard += value; _levelPair.EventSelectedCard += value; } remove { _levelSingle.EventSelectedCard -= value; _levelPair.EventSelectedCard -= value; } }
    public event Action<int> EventChangedAttempts { add { _levelSingle.EventChangedAttempts += value; _levelPair.EventChangedAttempts += value; } remove { _levelSingle.EventChangedAttempts -= value; _levelPair.EventChangedAttempts -= value; } }
    public event Action EventEndLevel { add { _levelSingle.EventEndLevel += value; _levelPair.EventEndLevel += value; } remove { _levelSingle.EventEndLevel -= value; _levelPair.EventEndLevel -= value; } }

#if UNITY_EDITOR
    private void Start()
    {
        if (_type == BonusLevelTypes.Single)
        {
            StartCoroutine(StartSingle_Coroutine(6, new(5, 2, 12), 3, false, 1));
            _levelSingle.EventSelectedCard += (t) => Debug.Log("+" + t);
            _levelSingle.EventEndLevel += () => StartCoroutine(StartSingle_Coroutine(URandom.Range(4, 12), new(0, 3, 4), 3, URandom.Range(0, 100) < 50, URandom.Range(0, 2)));
        }
        else
        {
            StartCoroutine(StartPair_Coroutine(5, new(1, 1, 5 * 2), 10, false));
            _levelPair.EventSelectedCard += (t) => Debug.Log("+" + t);
            _levelPair.EventEndLevel += () => { int i = URandom.Range(4, 8); StartCoroutine(StartPair_Coroutine(i, new(1, 1, i * 4), i * 2, URandom.Range(0, 100) < 50)); };
        }
    }
#endif

    private void Awake()
    {
        WaitForSeconds waitShowEndLevel = new(_timeShowEndLevel);

        _cardsArea = GetComponent<TimeCardsArea>();
        _thisGrid = GetComponent<GridLayoutGroup>();
        _defaultSpacing = _thisGrid.spacing;
        _sizeArea = GetComponent<RectTransform>().rect.size - _defaultSpacing * 2;

        _levelSingle = GetComponent<BonusLevelSingle>();
        _levelPair = GetComponent<BonusLevelPair>();

        _levelSingle.Initialize(_cardsArea, waitShowEndLevel);
        _levelPair.Initialize(_cardsArea, waitShowEndLevel);
    }

    public IEnumerator StartSingle_Coroutine(int size, Increment range, int attempts, bool isMonochrome, int countShuffle)
    {
        int countShapes = size * size;
        float cellSize = GridSetup(size);

        _levelSingle.Setup(size, attempts, _delayOpenPerAll / countShapes, _delayTurnPerAll / countShapes);
        return _levelSingle.StartRound_Coroutine(size, cellSize, GetBonusTime(range, isMonochrome), countShuffle);
    }

    public IEnumerator StartPair_Coroutine(int size, Increment range, int attempts, bool isMonochrome)
    {
        int countShapes = size * size;
        float cellSize = GridSetup(size);
        
        _levelPair.Setup(size, attempts, _delayOpenPerAll / countShapes, _delayTurnPerAll / countShapes);
        return _levelPair.StartRound_Coroutine(size, cellSize, GetBonusTime(range, isMonochrome));
    }

    private float GridSetup(int size)
    {
        Vector2 cellSize = _sizeArea / size;

        _thisGrid.constraintCount = size;
        _thisGrid.cellSize = cellSize;
        _thisGrid.spacing = _defaultSpacing / (size - 1);

        return cellSize.x;
    }

    private Queue<BonusTime> GetBonusTime(Increment range, bool isMonochrome)
    {
        Queue<BonusTime> shapes = new(range.Count);
        BonusTime shape;
        Color color = Color.white; color.Randomize(_saturationMin, _brightnessMin);

        while (range.TryGetNext(out int value))
        {
            shape = new(value, color);
            if (!isMonochrome)
                shape.SetUniqueColor(shapes, _saturationMin, _brightnessMin);
            shapes.Enqueue(shape);
        }

        return shapes;
    }
}
