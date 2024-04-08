using System;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private InputController _controller;
    [Space]
    [SerializeField] private GameArea _gameArea;
    [SerializeField] private GameUI _gameUI;
    [Space]
    [SerializeField] private float _timeSwitching = 1.1f;
    [SerializeField] private float _timeReset = 2f;
    [SerializeField] private float _timeGameOverMessage = 3f;

    private DataGame _dataGame;
    private PoolBlocks _poolBlocks;

    private WaitForSecondsRealtime _waitSwitching;
    private WaitForSecondsRealtime _waitReset;
    private WaitForSecondsRealtime _waitGameOverMessage;

    public event Action EventStartGame;
    public event Action EventGameOver;

    private void Awake()
    {
        _dataGame = DataGame.Instance;
        _poolBlocks = PoolBlocks.Instance;

        _waitSwitching = new(_timeSwitching);
        _waitReset = new(_timeReset);
        _waitGameOverMessage = new(_timeGameOverMessage);

        _controller.EventNewGame += OnNewGame;
        _controller.EventContinueGame += OnContinueGame;
        _controller.EventResetGame += OnResetGame;
        _controller.EventPause += OnPause;

        _gameArea.EventChangingGame += Save;
        _gameArea.EventGameOver += OnGameOver;
    }

    private void Start()
    {
        _gameArea.Initialize();
        _poolBlocks.Setup(_dataGame.MaxDigit);

        _controller.ControlEnableGame = false;
        Time.timeScale = 0.000000001f;
    }

    private void OnNewGame()
    {
        StartCoroutine(OnNewGameCoroutine());

        #region Local Function
        IEnumerator OnNewGameCoroutine()
        {
            EventStartGame?.Invoke();
            _controller.ControlEnable = false;
            _gameArea.Open();
            _gameUI.Close();

            _dataGame.StartNewGame();
            _poolBlocks.Setup(_dataGame.MaxDigit);
            _gameArea.Setup();

            yield return _waitSwitching;

            Time.timeScale = 1f;
            yield return StartCoroutine(_gameArea.StartWorkCoroutine());

            _controller.ControlEnable = true;
        }
        #endregion
    }

    private void OnContinueGame()
    {
        StartCoroutine(OnContinueGameCoroutine());

        #region Local Function
        IEnumerator OnContinueGameCoroutine()
        {
            _controller.ControlEnable = false;
            _gameArea.Open();
            _gameUI.Close();

            yield return _waitSwitching;
                        
            Time.timeScale = 1f;
            yield return StartCoroutine(_gameArea.StartWorkCoroutine());

            _controller.ControlEnable = true;
        }
        #endregion
    }

    private void OnResetGame()
    {
        StartCoroutine(OnResetGameCoroutine());

        #region Local Function
        IEnumerator OnResetGameCoroutine()
        {
            _controller.ControlEnable = false;
            _gameArea.Open();
            _gameUI.Close();

            if (_dataGame.IsRecord)
                _gameUI.SetScore(_dataGame.Score);
           
            yield return _waitSwitching;

            Time.timeScale = 1f;
            _gameArea.ResetState();
            _dataGame.ResetGame();
            _dataGame.StartNewGame();
            Save();

            yield return _waitReset;

            _poolBlocks.Setup(_dataGame.MaxDigit);
            _gameArea.Setup();
           
            yield return StartCoroutine(_gameArea.StartWorkCoroutine());

            _controller.ControlEnable = true;
        }
        #endregion
    }

    private void OnGameOver()
    {
        StartCoroutine(OnGameOverCoroutine());

        #region Local Function
        IEnumerator OnGameOverCoroutine()
        {
            EventGameOver?.Invoke();
            _controller.ControlEnableGame = false;

            bool IsRecord = _dataGame.IsRecord;
            if (IsRecord)
                _gameUI.SetScore(_dataGame.Score);

            yield return _waitGameOverMessage;

            _gameArea.ResetState();
            _dataGame.ResetGame();
            Save();

            yield return _waitReset;
            _dataGame.ResetScoreEvent();

            _gameArea.Close();
            if (IsRecord)
                _gameUI.OpenLeaderboard();
            else
                _gameUI.Open();

            yield return _waitSwitching;
        }
        #endregion
    }

    private void OnPause()
    {
        StartCoroutine(OnPauseCoroutine());

        #region Local Function
        IEnumerator OnPauseCoroutine()
        {
            _controller.ControlEnable = false;
            Time.timeScale = 0.000000001f;

            _gameArea.Close();
            _gameUI.Open();

            yield return _waitSwitching;

            _controller.ControlEnable = true;
        }
        #endregion
    }

    private void Save()
    {
        _gameArea.Save();
        _dataGame.Save();
    }

}
