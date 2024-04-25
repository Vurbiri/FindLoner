using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private GameArea _gameArea;
    [Space]
    [SerializeField] private float _timePreStart = 1.5f;
    [Space]
    [SerializeField] private bool _isCheat = false;

    private DataGame _dataGame;
    private bool _isRecord;

    private void Awake()
    {
        _dataGame = DataGame.Instance;

        _gameUI.EventPause += OnPause;
        _gameUI.EventPlay += OnPlay;
        _gameUI.EventStart += GameStart;

        _gameUI.ControlEnable = false;

        _gameArea.Initialize();

        _gameArea.EventScoreAdd += _dataGame.ScoreAdd;

        _gameArea.EventEndGameLevel += StartNextBonusLevel;
        _gameArea.EventEndBonusLevel += StartNextGameLevel;

        _gameArea.EventGameLevelFail += ResetGame;
        _gameArea.EventGameOver += GameOver;

        Card.IsCheat = _isCheat;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(_timePreStart);
        _gameUI.ControlEnable = true;

        if (_dataGame.IsGameLevel)
            GameStart();
        else
            StartNextBonusLevel();

    }

    private void GameStart()
    {
        _gameArea.StartGameLevel(_dataGame.StartGameLevel(), _dataGame.Level);
    }

    private void ResetGame()
    {
        _gameUI.ControlEnable = false;

        _isRecord = false;
        if (_dataGame.IsRecord)
            _gameUI.SetScore(_dataGame.Score, (b) => _isRecord = b);

        _dataGame.ResetGame();
        _dataGame.Save();
    }

    private void GameOver()
    {
        _gameUI.ControlEnable = true;
        _dataGame.Score = 0;
        if (_isRecord)
        {
            _gameUI.OpenLeaderboard();
            return;
        }

        GameStart();
    }

    private void StartNextGameLevel(float time)
    {
        _gameArea.StartGameLevel(_dataGame.NextGameLevel(time), _dataGame.Level);
    }

    private void StartNextBonusLevel()
    {
        _dataGame.CalkGameLevelData();

        if (_dataGame.IsBonusLevelSingle)
            _gameArea.StartBonusLevelSingle(_dataGame.NextBonusLevel());
        else
            _gameArea.StartBonusLevelPair(_dataGame.NextBonusLevel());
    }

    private void OnPause()
    {
        _gameArea.ControlEnable = false;
        Time.timeScale = 0;
    }

    private void OnPlay()
    {
        Time.timeScale = 1;
        _gameArea.ControlEnable = true;
    }
}
