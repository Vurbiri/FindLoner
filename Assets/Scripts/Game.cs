using System;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameArea _gameArea;
    [Space]
    [SerializeField] private float _startTime = 20;
    [Space]
    [SerializeField] private int _startSize = 4;
    [SerializeField] private int _startTypes = 4;
    [SerializeField] private int _maxSize = 12;
    
    private DataGame _dataGame;
    private int _currentSize, _currentSqrSize, _currentTypes, _maxTypes;
    private bool _isMonochrome = false;


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

        if(_dataGame.Level % 2 == 0)
            _gameArea.StartBonusLevelSingle(new(_startTime, _currentSize, _currentSize, _isMonochrome, new(_maxTypes), _currentSize * 2));
        else
            _gameArea.StartBonusLevelPair(new(_startTime, _currentSize, _maxTypes, _isMonochrome, new(_maxTypes - 1)));
    }

    private void CalkGameLevelData()
    {
        _isMonochrome = !_isMonochrome;

        if (_isMonochrome || _currentSize == _maxSize)
            return;

        if ((_currentTypes += 2) <= _maxTypes)
            return;

        _currentSize++;
        _currentSqrSize = _currentSize * _currentSize;
        _currentTypes = _startTypes;
        _maxTypes = _currentSqrSize / 2;
    }
}
