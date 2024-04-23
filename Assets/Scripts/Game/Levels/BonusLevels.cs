using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BonusLevelSingle), typeof(BonusLevelPair))]
[RequireComponent(typeof(TimeCardsArea))]
public class BonusLevels : MonoBehaviour
{
    [SerializeField] private float _timeShowEndLevel = 1.75f;
    [Space]
    [SerializeField] private float _timeOpenPerAll = 1.5f;
    [SerializeField] private float _timeTurnPerAll = 2.5f;
    [Space]
    [SerializeField] private float _saturationMin = 0.275f;
    [SerializeField] private float _brightnessMin = 0.25f;

    private BonusLevelSingle _levelSingle;
    private BonusLevelPair _levelPair;
    private ABonusLevel _levelCurrent;

    private float _time;

    private TimeCardsArea _cardsArea;
       
    public event Action<int> EventSetMaxAttempts;
    public event Action<float> EventSetTime;
    public event Action<float> EventAddTime;
    public event Action<int> EventChangedAttempts { add { _levelSingle.EventChangedAttempts += value; _levelPair.EventChangedAttempts += value; } remove { _levelSingle.EventChangedAttempts -= value; _levelPair.EventChangedAttempts -= value; }}
    public event Action<float> EventEndLevel;

    public void Initialize(float sizeArea, float startSpacing)
    {
        WaitForSeconds waitShowEndLevel = new(_timeShowEndLevel);

        _cardsArea = GetComponent<TimeCardsArea>();
        _cardsArea.Initialize(sizeArea, startSpacing);

        _levelSingle = GetComponent<BonusLevelSingle>();
        _levelPair = GetComponent<BonusLevelPair>();

        _levelSingle.EventSelectedCard += AddTime;
        _levelPair.EventSelectedCard += AddTime;

        _levelSingle.EventEndLevel += OnEndLevel;
        _levelPair.EventEndLevel += OnEndLevel;

        _levelSingle.Initialize(_cardsArea, waitShowEndLevel);
        _levelPair.Initialize(_cardsArea, waitShowEndLevel);

        #region Local function
        //======================
        void AddTime(float addTime)
        {
            _time += addTime;
            EventAddTime?.Invoke(_time);
        }
        void OnEndLevel()
        {
            EventEndLevel?.Invoke(_time);
        }
        #endregion
    }

    public IEnumerator StartLevelSingle_Coroutine(LevelSetupData data) => StartLevel_Coroutine(_levelSingle, data);
    public IEnumerator StartLevelPair_Coroutine(LevelSetupData data) => StartLevel_Coroutine(_levelPair, data);
    private IEnumerator StartLevel_Coroutine(ABonusLevel level, LevelSetupData data)
    {
        _levelCurrent = level;
        _time = data.Time;

        EventSetMaxAttempts?.Invoke(data.Count);
        EventSetTime.Invoke(data.Time);

        level.Setup(data, _timeOpenPerAll / data.CountShapes, _timeTurnPerAll / data.CountShapes);
        return level.StartRound_Coroutine(GetBonusTime(data.Range, data.IsMonochrome), data.CountShuffle);

        #region Local function
        //=========================================================
        Queue<BonusTime> GetBonusTime(Increment range, bool isMonochrome)
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
        #endregion
    }

    public void Run() => _levelCurrent.Run();
}
