using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BonusLevelSingle), typeof(BonusLevelPair))]
[RequireComponent(typeof(TimeCardsArea), typeof(Timer))]
public class BonusLevels : MonoBehaviour
{
    [SerializeField] protected float _ratioTimeShowStartLevel = 0.1f;
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

    private TimeCardsArea _cardsArea;
    
    public event Action<float> EventSetTime { add { _levelSingle.EventSetTime += value; _levelPair.EventSetTime += value; } remove { _levelSingle.EventSetTime -= value; _levelPair.EventSetTime -= value; } }
    public event Action<float> EventAddTime { add { _levelSingle.EventAddTime += value; _levelPair.EventAddTime += value; } remove { _levelSingle.EventAddTime -= value; _levelPair.EventAddTime -= value; } }

    public event Action<int> EventSetMaxAttempts { add { _levelSingle.EventSetMaxAttempts += value; _levelPair.EventSetMaxAttempts += value; } remove { _levelSingle.EventSetMaxAttempts -= value; _levelPair.EventSetMaxAttempts -= value; } }
    public event Action<int> EventChangedAttempts { add { _levelSingle.EventChangedAttempts += value; _levelPair.EventChangedAttempts += value; } remove { _levelSingle.EventChangedAttempts -= value; _levelPair.EventChangedAttempts -= value; }}
    public event Action<float> EventEndLevel { add { _levelSingle.EventEndLevel += value; _levelPair.EventEndLevel += value; } remove { _levelSingle.EventEndLevel -= value; _levelPair.EventEndLevel -= value; } }

    public void Initialize(float sizeArea, float startSpacing)
    {
        WaitForSeconds waitShowEndLevel = new(_timeShowEndLevel);
        Timer timer = GetComponent<Timer>();

        _cardsArea = GetComponent<TimeCardsArea>();
        _cardsArea.Initialize(sizeArea, startSpacing);

        _levelSingle = GetComponent<BonusLevelSingle>();
        _levelPair = GetComponent<BonusLevelPair>();

        _levelSingle.Initialize(_cardsArea, timer, waitShowEndLevel);
        _levelPair.Initialize(_cardsArea, timer, waitShowEndLevel);

        #region Local function
        //======================

        #endregion
    }

    public IEnumerator StartLevelSingle_Coroutine(LevelSetupData data) => StartLevel_Coroutine(_levelSingle, data);
    public IEnumerator StartLevelPair_Coroutine(LevelSetupData data) => StartLevel_Coroutine(_levelPair, data);
    private IEnumerator StartLevel_Coroutine(ABonusLevel level, LevelSetupData data)
    {
        _levelCurrent = level;

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

    public float TimeShowStart(float count) => _ratioTimeShowStartLevel * count;
}
