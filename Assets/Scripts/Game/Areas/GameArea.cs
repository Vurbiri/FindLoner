using System;
using System.Collections;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private float _size = 920f;
    [SerializeField] private float _startSpacing = 12f;
    [Space]
    [SerializeField] private GameLevel _gameLevel;
    [SerializeField] private BonusLevels _bonusLevels;
    [Space]
    [SerializeField] private Timer _timer;
    [Space]
    [SerializeField] private ScreenMessage _screenMessage;

    public event Action EventScoreAdd;
    public event Action<bool> EventEndGameLevel { add { _gameLevel.EventEndLevel += value; }  remove { _gameLevel.EventEndLevel -= value; } }
    public event Action<float> EventEndBonusLevel { add { _bonusLevels.EventEndLevel += value; } remove { _bonusLevels.EventEndLevel -= value; } }

    private void Awake()
    {
        _gameLevel.Initialize(_size, _startSpacing);
        _bonusLevels.Initialize(_size, _startSpacing);

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
            yield return _screenMessage.GameLevel_Wait();
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
            yield return _screenMessage.BonusLevelSingle_Wait();
            yield return StartCoroutine(_bonusLevels.StartLevelSingle_Coroutine(data));
            _bonusLevels.Run();
        }
    }

    public void StartBonusLevelPair(LevelSetupData data)
    {
        StartCoroutine(StartBonusLevelPair_Coroutine());

        IEnumerator StartBonusLevelPair_Coroutine()
        {
            yield return _screenMessage.BonusLevelPair_Wait();
            yield return StartCoroutine(_bonusLevels.StartLevelPair_Coroutine(data));
            _bonusLevels.Run();
        }
    }
}
