using Newtonsoft.Json;
using System;
using UnityEngine;

//[DefaultExecutionOrder(-1)]
public class DataGame : ASingleton<DataGame>
{
    private const string KEY = "gmd_test";
    
    [Space]
    [SerializeField, Range(1, 5)] private int _startCountBombs = 3;
    [SerializeField] private int _bombPerScoreBase = 1750;
    [SerializeField] private int _penaltyWeightBase = 40;
    [Space]
    [SerializeField] private int[] _scorePerStage;
    [Space]
    [SerializeField] private GameSettings _settings;
    [SerializeField, Range(3, 8)] private int _minDigit = 5;

    private const int REVERS_MAX_DIGIT = 10;

    private GameSave _data;
    private int _countBombAdd = 1;
    private int _difficulty = 0;
    private bool _isNewRecord = false;
    private int BombPerScore => _bombPerScoreBase * (REVERS_MAX_DIGIT - MaxDigit);

    public bool IsNewGame => _data.modeStart == GameModeStart.New; 
    public bool IsRecord => _data.score > _data.maxScore;
    public long Score { get => _data.score; private set { _data.score = value; EventChangeScore?.Invoke(value); } }
    public long MaxScore { get => _data.maxScore; private set { _data.maxScore = value; EventChangeMaxScore?.Invoke(value.ToString()); } }
    public NextShape[] NextShapes { get => _data.nextShapes; set => _data.nextShapes = value; }
    public int MinDigit => _minDigit;
    public int MaxDigit { get => _data.maxDigit; set { _data.maxDigit = value; _difficulty = value  - _minDigit; } }
    public int CountBombs { get => _data.countBombs; set { _data.countBombs = value; EventChangeCountBombs?.Invoke(value); } }
    public int CountBonusWeight => 5;
    public int BonusWeight => _difficulty * _difficulty * _difficulty * (_data.maxDigit - Stage);
    public int PenaltyWeightLast => - _penaltyWeightBase * (_difficulty > 1 ? 1 : 0);
    public int Stage { get; private set; }
    public int CountShapesEmpty  { get; private set; }
    public int Difficulty => _difficulty;
    public int[,] SaveArea => _data.area;

    public event Action<long> EventChangeScore;
    public event Action<string> EventChangeMaxScore;
    public event Action<int> EventChangeCountBombs;
    public event Action EventNewRecord;
    public event Action EventNewStage;

    public bool Initialize(bool isLoad)
    {
        bool result = isLoad && Load();

        if (!result)
            _data = new(_settings, _startCountBombs);

        InitParameters();

        return result;

        #region Local Function
        void InitParameters()
        {
            _difficulty = _data.maxDigit - _minDigit;
            _countBombAdd = Mathf.FloorToInt(Score / BombPerScore) + 1;
            CalkNewStage();
            _isNewRecord = IsRecord;
        }
        #endregion
    }

    private bool Load()
    {
        Return<GameSave> data = Storage.Load<GameSave>(KEY);
        if (data.Result)
            _data = data.Value;

        return data.Result;
    }
    public void Save() => Storage.Save(KEY, _data);

    public void StartNewGame()
    {
        _data.modeStart = GameModeStart.Continue;
        EventChangeScore?.Invoke(0);
        CountBombs = _startCountBombs;
    }

    public void ResetGame()
    {
        if(IsRecord)
            MaxScore = Score;

        CountBombs = 0;
        Stage = 0;
        CountShapesEmpty = 1;

        _data.modeStart = GameModeStart.New;
        _data.score = 0;

        _countBombAdd = 1;
        _isNewRecord = false;
    }

    public void ResetScoreEvent() => EventChangeScore?.Invoke(0);

    public void ScoreForAdd(int value)
    {
        Score += value + _difficulty;
        CheckChangeScore();
    }

    public void ScoreForSeries(int digit, int countSeries, int countOne)
    {
        Score += digit * (2 * countSeries + countOne - digit) * 2;
        CheckChangeScore();
    }

    private void CheckChangeScore()
    {
        if (!_isNewRecord && IsRecord && _data.maxScore > 0)
        {
            _isNewRecord = true;
            EventNewRecord?.Invoke();
        }

        if (Score > _countBombAdd * BombPerScore)
        {
            CountBombs++;
            _countBombAdd++;
        }

        if (CalkNewStage())
            EventNewStage?.Invoke();
    }

    private bool CalkNewStage()
    {
        for(int i = _scorePerStage.Length - 1; i >= 0; i--)
        {
            if(Score > _scorePerStage[i])
            {
                if (Stage <= i)
                {
                    Stage = i + 1;
                    CountShapesEmpty = Mathf.Clamp(i + 2, 1, 3);
                    return true;
                }

                return false;
            }

        }
        
        Stage = 0;
        CountShapesEmpty = 1;

        return false;
    }

    #region Nested Classe
    private class GameSave : GameSettings
    {
        [JsonProperty("gms")]
        public GameModeStart modeStart = GameModeStart.New;
        [JsonProperty("scr")]
        public long score = 0;
        [JsonProperty("msc")]
        public long maxScore = 0;
        [JsonProperty("bmb")]
        public int countBombs = 3;
        [JsonProperty("nsh")]
        public NextShape[] nextShapes = new NextShape[NextShape.count];
        [JsonProperty("are")]
        public int[,] area = new int[BlocksArea.size, BlocksArea.size];
               
        [JsonConstructor]
        public GameSave(int maxDigit, GameModeStart modeStart,  long score, long maxScore, int countBombs, NextShape[] nextShapes, int[,] area) 
            : base(maxDigit)
        {
            this.modeStart = modeStart;
            this.score = score;
            this.maxScore = maxScore;
            this.countBombs = countBombs;
            this.nextShapes = nextShapes;
            this.area = area;
        }
        public GameSave(GameSettings gameSettings, int countBombs) : base(gameSettings) 
        {
            modeStart = GameModeStart.New;
            score = 0;
            maxScore = 0;
            this.countBombs = countBombs;
            nextShapes = new NextShape[NextShape.count];
            area = new int[BlocksArea.size, BlocksArea.size];
        }
    }

    [System.Serializable]
    private class GameSettings
    {
        [JsonProperty("mdg"), Range(4, 9)]
        public int maxDigit = 7;
        

        public GameSettings(int maxDigit)
        {
            this.maxDigit = maxDigit;
        }

        public GameSettings(GameSettings gameSettings) 
        {
            maxDigit = gameSettings.maxDigit;
        }
    }
     #endregion
}
