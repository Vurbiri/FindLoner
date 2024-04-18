using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BonusLevelSingle), typeof(BonusLevelPair))]
[RequireComponent(typeof(TimeCardsArea), typeof(GridLayoutGroup))]
public class BonusLevels : MonoBehaviour, ILevelPlay
{
    [SerializeField] private float _timeShowEndLevel = 1.75f;
    [Space]
    [SerializeField] private float _timeOpenPerAll = 1.5f;
    [SerializeField] private float _timeTurnPerAll = 2.5f;
    [Space]
    [SerializeField] private float _saturationMin = 0.275f;
    [SerializeField] private float _brightnessMin = 0.25f;
#if UNITY_EDITOR
    [Space]
    [SerializeField] private BonusLevelTypes _type;
#endif

    private BonusLevelSingle _levelSingle;
    private BonusLevelPair _levelPair;
    private ABonusLevel _levelCurrent;

    private int _time;

    private TimeCardsArea _cardsArea;
    private GridLayoutGroup _thisGrid;
    private Vector2 _sizeArea, _defaultSpacing;

    //public event Action<int> EventAddTime { add { _levelSingle.EventSelectedCard += value; _levelPair.EventSelectedCard += value; } remove { _levelSingle.EventSelectedCard -= value; _levelPair.EventSelectedCard -= value; } }
    public event Action<int> EventSetMaxAttempts;
    public event Action<int> EventSetTime;
    public event Action<int> EventAddTime;
    public event Action<int> EventChangedAttempts { add { _levelSingle.EventChangedAttempts += value; _levelPair.EventChangedAttempts += value; } remove { _levelSingle.EventChangedAttempts -= value; _levelPair.EventChangedAttempts -= value; }}
    public event Action EventEndLevel { add { _levelSingle.EventEndLevel += value; _levelPair.EventEndLevel += value; } remove { _levelSingle.EventEndLevel -= value; _levelPair.EventEndLevel -= value; } }
    

#if UNITY_EDITOR
    public void StartLevel()
    {
        if (_type == BonusLevelTypes.Single)
        {
            StartCoroutine(StartSingle_Coroutine(new(6, 3, false, new(5, 2, 12), 60000, 3)));
            _levelSingle.EventSelectedCard += (t) => Debug.Log("+" + t);
            _levelSingle.EventEndLevel += () => StartCoroutine(StartSingle_Coroutine(new(BonusLevelTypes.Single)));
        }
        else
        {
            StartCoroutine(StartPair_Coroutine(new(5, 10, false, new(1, 1, 5 * 2), 60000)));
            _levelPair.EventSelectedCard += (t) => Debug.Log("+" + t);
            _levelPair.EventEndLevel += () => StartCoroutine(StartPair_Coroutine(new(BonusLevelTypes.Pair)));
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

        _levelSingle.EventSelectedCard += AddTime;
        _levelPair.EventSelectedCard += AddTime;

        _levelSingle.Initialize(_cardsArea, waitShowEndLevel);
        _levelPair.Initialize(_cardsArea, waitShowEndLevel);

        void AddTime(int time)
        {
            _time += time;
            EventAddTime?.Invoke(_time);
        }
    }

    public void Play() => _levelCurrent.Play();

    public IEnumerator StartSingle_Coroutine(BonusLevelSetupData data)
    {
        Setup(_levelSingle, data.Count, data.Time);

        float cellSize = GridSetup(data.Size);

        _levelSingle.Setup(data, _timeOpenPerAll / data.CountShapes, _timeTurnPerAll / data.CountShapes);
        return _levelSingle.StartRound_Coroutine(data.Size, cellSize, GetBonusTime(data.Range, data.IsMonochrome), data.CountShuffle);
    }

    public IEnumerator StartPair_Coroutine(BonusLevelSetupData data)
    {
        Setup(_levelPair, data.Count, data.Time);

        float cellSize = GridSetup(data.Size);
        
        _levelPair.Setup(data, _timeOpenPerAll / data.CountShapes, _timeTurnPerAll / data.CountShapes);
        return _levelPair.StartRound_Coroutine(data.Size, cellSize, GetBonusTime(data.Range, data.IsMonochrome));
    }

    private void Setup(ABonusLevel level,  int attempts, int time)
    {
        _levelCurrent = level;
        _time = time;
        EventSetMaxAttempts?.Invoke(attempts);
        EventSetTime.Invoke(time);
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
        Queue<BonusTime> bonuses = new(range.Count);
        BonusTime bonus;
        Color color = Color.white; color.Randomize(_saturationMin, _brightnessMin);

        while (range.TryGetNext(out int value))
        {
            bonus = new(value, color);
            if (!isMonochrome)
                bonus.SetUniqueColor(bonuses, _saturationMin, _brightnessMin);
            bonuses.Enqueue(bonus);
        }

        return bonuses;
    }
}
