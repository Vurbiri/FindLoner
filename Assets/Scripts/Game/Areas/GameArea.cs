using System;
using System.Collections;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private GameLevel _gameLevel;
    [SerializeField] private BonusLevels _bonusLevels;
    [Space]
    [SerializeField] private Timer _timer;

    public event Action EventScoreAdd;
    public event Action<bool> EventEndGameLevel { add { _gameLevel.EventEndLevel += value; }  remove { _gameLevel.EventEndLevel -= value; } }
    public event Action<float> EventEndBonusLevel { add { _bonusLevels.EventEndLevel += value; } remove { _bonusLevels.EventEndLevel -= value; } }

    private void Awake()
    {
        _timer.EventEndTime += _gameLevel.Stop;

        _gameLevel.EventStartRound += () => _timer.IsPause = false;
        _gameLevel.EventEndRound += OnEndRound;

        #region Local function
        //======================
        void OnEndRound(bool isContinue)
        {
            if (isContinue)
            {
                _timer.IsPause = true;
                EventScoreAdd?.Invoke();
            }
            else
            {
                _timer.Stop();
            }
        }
        #endregion
    }

    public void StartGameLevel(LevelSetupData data)
    {
        StartCoroutine(StartGameLevel_Coroutine());

        IEnumerator StartGameLevel_Coroutine()
        {
            _timer.MaxTime = data.Time;
            yield return _gameLevel.StartLevel_Routine(data);
            _gameLevel.Run();
            _timer.Run();
        }
    }

    public void StartBonusLevelSingle(LevelSetupData data)
    {
        StartCoroutine(StartBonusLevelSingle_Coroutine());

        IEnumerator StartBonusLevelSingle_Coroutine()
        {
            yield return StartCoroutine(_bonusLevels.StartLevelSingle_Coroutine(data));
            _bonusLevels.Run();
        }
    }

    public void StartBonusLevelPair(LevelSetupData data)
    {
        StartCoroutine(StartBonusLevelPair_Coroutine());

        IEnumerator StartBonusLevelPair_Coroutine()
        {
            yield return StartCoroutine(_bonusLevels.StartLevelPair_Coroutine(data));
            _bonusLevels.Run();
        }
    }
}
