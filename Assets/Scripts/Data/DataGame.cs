using Newtonsoft.Json;
using System;
using UnityEngine;

//[DefaultExecutionOrder(-1)]
public class DataGame : ASingleton<DataGame>
{
    [Space]
    [SerializeField] private string _keySave = "gmd";
    [Space]
    [SerializeField] private float _timeStart = 11;
    [Space]
    [SerializeField] private int _startSize = 3;
    [SerializeField] private int _maxSize = 8;
    [Space]
    [SerializeField] private int _startTypes = 4;
    [SerializeField] private int _stepTypesOne = 1;
    [SerializeField] private int _stepTypesTwo = 2;
    [SerializeField] private int _levelStepOneToTwo = 7;
    [Space]
    [SerializeField] private int _scoreStart = 4;
    [SerializeField] private int _scorePerLevel = 1;

    private GameSave _data;
    private bool _isNewRecord = false;
    private int _currentSize, _currentTypes, _maxTypes, _sqrCurrentSize;
    private bool _isMonochrome = false, _isBonusLevelSingle = true;

    private float TimeStart => _timeStart + _maxTypes + _currentTypes * (_isMonochrome ? 3.5f : 1.5f);

    public bool IsGameLevel => _data.modeStart == GameMode.Game;
    public bool IsBonusLevelSingle => _isBonusLevelSingle;
    public bool IsRecord => _data.score > _data.maxScore;
    public int Level => _data.level;
    public int Score { get => _data.score; set { _data.score = value; EventChangeScore?.Invoke(value); } }
    public int MaxScore { get => _data.maxScore; private set { _data.maxScore = value; EventChangeMaxScore?.Invoke(value); } }

    public event Action<int> EventChangeScore;
    public event Action<int> EventChangeMaxScore;

    public bool Initialize(bool isLoad)
    {
        ResetGameLevelData();

        bool result = isLoad && Load();
        if (!result)
        {
            _data = new();
            ResetGame();
        }

        _isNewRecord = IsRecord;

        
        for (int i = 1; i < _data.level; i++)
            CalkGameLevelData();

        return result;
    }

    private bool Load()
    {
        Return<GameSave> data = Storage.Load<GameSave>(_keySave);
        if (data.Result)
            _data = data.Value;

        return data.Result;
    }
    public void Save(Action<bool> callback = null) => StartCoroutine(Storage.Save_Coroutine(_keySave, _data, callback));

    public LevelSetupData StartGameLevel() => new(_data.time, _currentSize, _currentTypes, _isMonochrome);
    public LevelSetupData NextGameLevel(float time)
    {
        _data.modeStart = GameMode.Game;
        _data.level++;
        _data.time = time;
        Save(MessageSaving);

        return new(time, _currentSize, _currentTypes, _isMonochrome);
    }
    public LevelSetupData NextBonusLevel()
    {
        _data.modeStart = GameMode.Bonus;
        Save(MessageSaving);

        if (_isBonusLevelSingle)
            return new(TimeStart, _currentSize, Mathf.RoundToInt(0.64f * (_currentTypes + _currentSize)), !_isMonochrome, _currentSize + 2, new(_maxTypes - 1));
        else
            return new(TimeStart, _currentSize, _currentTypes + _maxTypes, !_isMonochrome, 0, new(_maxTypes - 1));
    }

    public void ResetGame()
    {
        if (IsRecord)
            MaxScore = Score;

        ResetGameLevelData();
        _data.Reset(TimeStart);
        _isNewRecord = false;
    }

    public void ResetScoreEvent() => EventChangeScore?.Invoke(0);

    public void ScoreAdd()
    {
        Score += _scoreStart + _scorePerLevel * _data.level;

        if (!_isNewRecord && IsRecord && _data.maxScore > 0)
            _isNewRecord = true;
    }

    public void CalkGameLevelData()
    {
        _isMonochrome = !_isMonochrome;

        if (_isMonochrome)
            return;

        _isBonusLevelSingle = !_isBonusLevelSingle;

        if ((_currentTypes += _currentSize >= _levelStepOneToTwo ? _stepTypesTwo : _stepTypesOne) <= _maxTypes)
            return;

        if (_currentSize == _maxSize)
        {
            _currentTypes = _maxTypes;
            return;
        }

        _currentSize++;
        _sqrCurrentSize = _currentSize * _currentSize;
        _currentTypes = _startTypes;
        _maxTypes = _sqrCurrentSize >> 1;
    }

    public void ResetGameLevelData()
    {
        _isMonochrome = false;
        _isBonusLevelSingle = true;
        _currentSize = _startSize;
        _sqrCurrentSize = _currentSize * _currentSize;
        _currentTypes = _startTypes;
        _maxTypes = _sqrCurrentSize >> 1;
    }

    private void MessageSaving(bool result) => Message.Saving("GoodSave", result);

    #region Nested Classe
    //***********************************
    private class GameSave
    {
        [JsonProperty("gms")]
        public GameMode modeStart = GameMode.Game;
        [JsonProperty("lvl")]
        public int level = 1;
        [JsonProperty("scr")]
        public int score = 0;
        [JsonProperty("msc")]
        public int maxScore = 0;
        [JsonProperty("tme")]
        public float time = 0;

        [JsonConstructor]
        public GameSave(GameMode modeStart, int level, int score, int maxScore, float time) 
        {
            this.modeStart = modeStart;
            this.level = level;
            this.score = score;
            this.maxScore = maxScore;
            this.time = time;
        }
        public GameSave()
        {
            modeStart = GameMode.Game;
            level = 1;
            score = 0;
            time = 0;
            maxScore = 0;
        }

        public void Reset(float time)
        {
            modeStart = GameMode.Game;
            level = 1;
            score = 0;
            this.time = time;
        }
    }
    #endregion
}
