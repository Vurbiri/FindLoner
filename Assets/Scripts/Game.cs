using System;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameArea _gameArea;
    [Space]
    [SerializeField] private float _startTime = 10;
    [Space]
    [SerializeField] private int _startSize = 4;
    [SerializeField] private int _startTypes = 4;
    [SerializeField] private int _stepTypes = 2;
    [SerializeField] private int _maxSize = 12;
    
    private DataGame _dataGame;
    private int _currentSize, _currentSqrSize, _currentTypes, _maxTypes;
    private bool _isMonochrome = false, _isBonusSingle = true;


    private void Awake()
    {
        _dataGame = DataGame.Instance;

        _gameArea.EventScoreAdd += _dataGame.ScoreAdd;
        _gameArea.EventEndBonusLevel += StartNextGameLevel;
        _gameArea.EventEndGameLevel += OnEndGameLevel;

        Initialize();

        #region Local function
        //======================
        void OnEndGameLevel(bool isGameOver)
        {
            if (isGameOver)
                GameOver();
            else
                StartNextBonusLevel();
        }
        void Initialize()
        {
            _isMonochrome = false;
            _isBonusSingle = true;
            _currentSize = _startSize;
            _currentSqrSize = _currentSize * _currentSize;
            _currentTypes = _startTypes;
            _maxTypes = _currentSqrSize / 2;
        }
        #endregion
    }

    private void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        _gameArea.StartGameLevel(new(_startTime, _currentSize, _currentTypes, _isMonochrome));
    }

    private void GameOver()
    {
        Debug.Log("-= GameOver =-");
    }

    private void StartNextGameLevel(float time)
    {
        _dataGame.Level++;
        _dataGame.Time = time;
        //_dataGame.Save();

        CalkGameLevelData();

        _gameArea.StartGameLevel(new(time, _currentSize, _currentTypes, _isMonochrome));
    }

    private void StartNextBonusLevel()
    {
        //_dataGame.Save();

        if(_isBonusSingle)
            _gameArea.StartBonusLevelSingle(new(_startTime, _currentSize, _currentTypes, _isMonochrome, new(Mathf.FloorToInt(_maxTypes * 0.9f)), Mathf.FloorToInt(_currentSize * 1.4f)));
        else
            _gameArea.StartBonusLevelPair(new(_startTime, _currentSize, _currentTypes + 1, _isMonochrome, new(Mathf.FloorToInt(_maxTypes * 0.9f))));
    }

    private void CalkGameLevelData()
    {
        _isMonochrome = !_isMonochrome;

        if (_isMonochrome)
            return;

        _isBonusSingle = !_isBonusSingle;

        if ((_currentTypes += _stepTypes) <= _maxTypes)
            return;

        if (_currentSize == _maxSize)
        {
            _currentTypes = _maxTypes;
            return;
        }

        _currentSize++;
        _currentSqrSize = _currentSize * _currentSize;
        _currentTypes = _startTypes;
        _maxTypes = _currentSqrSize / 2;
    }
}
