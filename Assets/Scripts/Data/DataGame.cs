using Newtonsoft.Json;
using System;
using UnityEngine;

//[DefaultExecutionOrder(-1)]
public class DataGame : ASingleton<DataGame>
{
    [Space]
    [SerializeField] private string _keySave = "gmd_test";

    private GameSave _data;
    private bool _isNewRecord = false;

    public bool IsNewGame => _data.modeStart == GameModeStart.New; 
    public bool IsRecord => _data.score > _data.maxScore;
    public long Score { get => _data.score; private set { _data.score = value; EventChangeScore?.Invoke(value); } }
    public long MaxScore { get => _data.maxScore; private set { _data.maxScore = value; EventChangeMaxScore?.Invoke(value.ToString()); } }


    public event Action<long> EventChangeScore;
    public event Action<string> EventChangeMaxScore;
    public event Action EventNewRecord;

    public bool Initialize(bool isLoad)
    {
        bool result = isLoad && Load();

        if (!result)
            _data = new();

        _isNewRecord = IsRecord; ;

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

        _data.modeStart = GameModeStart.New;
        _data.score = 0;
        _isNewRecord = false;
    }

    public void ResetScoreEvent() => EventChangeScore?.Invoke(0);

    public void ScoreForAdd(int value)
    {
        Score += value;

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
        [JsonProperty("scr")]
        public long score = 0;
        [JsonProperty("msc")]
        public long maxScore = 0;
               
        [JsonConstructor]
        public GameSave(GameModeStart modeStart,  long score, long maxScore) 
        {
            this.modeStart = modeStart;
            this.score = score;
            this.maxScore = maxScore;
        }
        public GameSave()
        {
            modeStart = GameModeStart.New;
            score = 0;
            maxScore = 0;
        }
    }
    #endregion
}
