using Newtonsoft.Json;
using System;
using UnityEngine;

//[DefaultExecutionOrder(-1)]
public class DataGame : ASingleton<DataGame>
{
    [Space]
    [SerializeField] private string _keySave = "gmd_test";
    [Space]
    [SerializeField] private float _timeStart = 24;
    [SerializeField] private float _timePerLevel = 1;
    [Space]
    [SerializeField] private int _scoreStart = 8;
    [SerializeField] private int _scorePerLevel = 2;

    private GameSave _data;
    private bool _isNewRecord = false;

    public bool IsNewGame => _data.modeStart == GameModeStart.New; 
    public bool IsRecord => _data.score > _data.maxScore;
    public int Level { get => _data.level; set => _data.level = value; }
    public int Score { get => _data.score; private set { _data.score = value; EventChangeScore?.Invoke(value); } }
    public int MaxScore { get => _data.maxScore; private set { _data.maxScore = value; EventChangeMaxScore?.Invoke(value); } }
    public float StartTime => _timeStart + _timePerLevel * _data.level;
    public float Time { get => _data.time; set => _data.time = value; }


    public event Action<int> EventChangeScore;
    public event Action<int> EventChangeMaxScore;
    public event Action EventNewRecord;

    public bool Initialize(bool isLoad)
    {
        bool result = isLoad && Load();

        if (!result)
            _data = new();

        _isNewRecord = IsRecord;

        return result;
    }

    private bool Load()
    {
        Return<GameSave> data = Storage.Load<GameSave>(_keySave);
        if (data.Result)
            _data = data.Value;

        return data.Result;
    }
    public void Save() => StartCoroutine(Storage.SaveCoroutine(_keySave, _data));

    public void StartNewGame()
    {
        _data.modeStart = GameModeStart.Continue;
        EventChangeScore?.Invoke(0);
    }

    public void ResetGame()
    {
        if(IsRecord)
            MaxScore = Score;

        _data.Reset();
        _data.time = _timeStart + _timePerLevel;
        _isNewRecord = false;
    }

    public void ResetScoreEvent() => EventChangeScore?.Invoke(0);

    public void ScoreAdd()
    {
        Score += _scoreStart + _scorePerLevel * _data.level;

        if (!_isNewRecord && IsRecord && _data.maxScore > 0)
        {
            _isNewRecord = true;
            EventNewRecord?.Invoke();
        }
    }

    #region Nested Classe
    private class GameSave
    {
        [JsonProperty("gms")]
        public GameModeStart modeStart = GameModeStart.New;
        [JsonProperty("lvl")]
        public int level = 1;
        [JsonProperty("scr")]
        public int score = 0;
        [JsonProperty("msc")]
        public int maxScore = 0;
        [JsonProperty("tme")]
        public float time = 0;

        [JsonConstructor]
        public GameSave(GameModeStart modeStart, int level, int score, int maxScore, float time) 
        {
            this.modeStart = modeStart;
            this.level = level;
            this.score = score;
            this.maxScore = maxScore;
            this.time = time;
        }
        public GameSave()
        {
            Reset();
            maxScore = 0;
        }

        public void Reset()
        {
            modeStart = GameModeStart.New;
            level = 1;
            score = 0;
            time = 0;
        }
    }
    #endregion
}
