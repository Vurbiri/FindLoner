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

        //_gameLevel.EventStartRound += () => _timer.IsPause = false;
        _gameLevel.EventEndRound += OnEndRound;

        #region Local function
        //======================
        void OnEndRound(bool isContinue)
        {
            if (isContinue)
                EventScoreAdd?.Invoke();
            else
                _timer.Stop();
        }
        #endregion
    }

    public void StartGameLevel(LevelSetupData data)
    {
        _gameLevel.SetActive(true);

        StartCoroutine(StartGameLevel_Coroutine());

        IEnumerator StartGameLevel_Coroutine()
        {
            _timer.MaxTime = data.Time;
            yield return StartCoroutine(_gameLevel.StartLevel_Coroutine(data));
            _gameLevel.Run();
            _timer.Run();
        }
    }

    public void StartBonusLevelSingle(LevelSetupData data)
    {
        _bonusLevels.SetActive(true);

        StartCoroutine(StartBonusLevelSingle_Coroutine());

        IEnumerator StartBonusLevelSingle_Coroutine()
        {
            yield return StartCoroutine(_bonusLevels.StartLevelSingle_Coroutine(data));
            _bonusLevels.Run();
        }
    }

    public void StartBonusLevelPair(LevelSetupData data)
    {
        _bonusLevels.SetActive(true);

        StartCoroutine(StartBonusLevelPair_Coroutine());

        IEnumerator StartBonusLevelPair_Coroutine()
        {
            yield return StartCoroutine(_bonusLevels.StartLevelPair_Coroutine(data));
            _bonusLevels.Run();
        }
    }
}
