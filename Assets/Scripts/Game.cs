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
    private bool _isMonochrome = false, _isSimilar = true;


    private void Awake()
    {
        _dataGame = DataGame.Instance;

        _gameArea.Initialize();

        _gameArea.EventScoreAdd += _dataGame.ScoreAdd;

        _gameArea.EventEndBonusLevel += StartNextGameLevel;
        _gameArea.EventEndGameLevel += StartNextBonusLevel;

        _gameArea.EventGameLevelFail += ResetGame;
        _gameArea.EventGameOver += GameOver;

        Initialize();

        #region Local function
        //======================
        void Initialize()
        {
            _isMonochrome = false;
            _isSimilar = true;
            _currentSize = _startSize;
            _currentSqrSize = _currentSize * _currentSize;
            _currentTypes = _startTypes;
            _maxTypes = _currentSqrSize / 2;
        }
        #endregion
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        GameStart();
    }

    private void GameStart()
    {
        _gameArea.StartGameLevel(new(_startTime, _currentSize, _currentTypes, _isMonochrome, _isSimilar ? 0 : 1), 1);
    }

    private void ResetGame()
    {
        _dataGame.ResetGame();
        Debug.Log("-= ResetGame =-");
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

        _gameArea.StartGameLevel(new(time, _currentSize, _currentTypes, _isMonochrome, _isSimilar ? 0 : 1), _dataGame.Level);
    }

    private void StartNextBonusLevel()
    {
        //_dataGame.Save();

        if(!_isSimilar)
            _gameArea.StartBonusLevelSingle(new(_startTime, _currentSize, _currentTypes, _isMonochrome, _currentSize, new(_maxTypes - 1)));
        else
            _gameArea.StartBonusLevelPair(new(_startTime, _currentSize, _currentTypes * 2, _isMonochrome, 0, new(_maxTypes - 1)));
    }

    private void CalkGameLevelData()
    {
        _isMonochrome = !_isMonochrome;

        if (_isMonochrome)
            return;

        _isSimilar = !_isSimilar;

        if (!_isSimilar)
            return;

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
