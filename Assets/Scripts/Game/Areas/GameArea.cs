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
    [SerializeField] private Timer _timerGameLevel;
    [SerializeField] private Timer _timerBonusLevels;
    [Space]
    [SerializeField] private ScreenMessage _screenMessage;

    private WaitActivate _waitMessageGameOver = null;

    public bool ControlEnable { set => _gameLevel.ControlEnable = _bonusLevels.ControlEnable = value; }

    public event Action EventScoreAdd;
    public event Action EventGameLevelFail;
    public event Action EventEndGameLevel;
    public event Action EventGameOver;
    public event Action<float> EventEndBonusLevel { add { _bonusLevels.EventEndLevel += value; } remove { _bonusLevels.EventEndLevel -= value; } }

    public void Initialize()
    {
        _gameLevel.Initialize(_size, _startSpacing);
        _bonusLevels.Initialize(_size, _startSpacing);

        _timerGameLevel.EventEndTime += OnEndTime;

        _gameLevel.EventStartRound += () => _timerGameLevel.IsPause = false;
        _gameLevel.EventEndRound += OnEndRound;
        _gameLevel.EventEndLevel += OnEndGameLevel;
                
        #region Local function
        //======================
        void OnEndTime()
        {
            if (!_gameLevel.Stop())
            {
                EventGameLevelFail?.Invoke();
                _waitMessageGameOver = _screenMessage.GameOver();
            }
            else
            {
                _screenMessage.LevelComplete();
            }
        }
        //======================
        void OnEndRound(bool isContinue)
        {
            if (isContinue)
            {
                _timerGameLevel.IsPause = true;
                EventScoreAdd?.Invoke();
            }
            else
            {
                EventGameLevelFail?.Invoke();
                _timerGameLevel.Stop();
                _waitMessageGameOver = _screenMessage.GameOver();
            }
        }
        //======================
        void OnEndGameLevel(bool isGameOver)
        {
            if (isGameOver)
                StartCoroutine(GameOver_Coroutine());
            else
                EventEndGameLevel?.Invoke();
        }
        IEnumerator GameOver_Coroutine()
        {
            yield return _waitMessageGameOver;
            EventGameOver?.Invoke();
        }
        #endregion
    }

    public void StartGameLevel(LevelSetupData data, int level)
    {
        StartCoroutine(StartGameLevel_Coroutine());

        IEnumerator StartGameLevel_Coroutine()
        {
            _timerGameLevel.MaxTime = data.Time;
            yield return _screenMessage.GameLevel_Wait(level, Mathf.RoundToInt(data.Time));
            yield return _gameLevel.StartLevel_Routine(data);
            _gameLevel.Run();
            _timerGameLevel.Run();
        }
    }

    public void StartBonusLevelSingle(LevelSetupData data)
    {
        StartCoroutine(StartBonusLevelSingle_Coroutine());

        IEnumerator StartBonusLevelSingle_Coroutine()
        {
            _timerBonusLevels.MaxTime = _bonusLevels.TimeShowStart(data.CountShapes);
            yield return _screenMessage.BonusLevelSingle_Wait(data.Count);
            yield return StartCoroutine(_bonusLevels.StartLevelSingle_Coroutine(data));
            _bonusLevels.Run();
        }
    }

    public void StartBonusLevelPair(LevelSetupData data)
    {
        StartCoroutine(StartBonusLevelPair_Coroutine());

        IEnumerator StartBonusLevelPair_Coroutine()
        {
            _timerBonusLevels.MaxTime = _bonusLevels.TimeShowStart(data.CountShapes);
            yield return _screenMessage.BonusLevelPair_Wait(data.Count);
            yield return StartCoroutine(_bonusLevels.StartLevelPair_Coroutine(data));
            _bonusLevels.Run();
        }
    }
}
