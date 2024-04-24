using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABonusLevel : MonoBehaviour, ILevelPlay
{
    protected SoundSingleton _sound;
    protected TimeCardsArea _cardsArea;

    private float _time;
    private int _attempts;
    protected int _countShapes;
    protected float _delayOpen, _delayTurn;
    private readonly Increment _layers = new(-256, 0);

    protected WaitForSeconds _waitShowEndLevel;
    private Timer _timer;

    protected int Attempts { get => _attempts;  set { _attempts = value; EventChangedAttempts?.Invoke(value); } }

    public event Action<float> EventSetTime;
    public event Action<float> EventAddTime;
    public event Action<int> EventSetMaxAttempts;
    public event Action<int> EventChangedAttempts;
    public event Action<float> EventEndLevel;

    public void Initialize(TimeCardsArea cardsArea, Timer timer, WaitForSeconds waitShowEndLevel)
    {
        _sound = SoundSingleton.Instance;
        _cardsArea = cardsArea;
        _timer = timer;
        _waitShowEndLevel = waitShowEndLevel;
    }

    public void Setup(LevelSetupData data, float delayOpen, float delayTurn)
    {
        _delayOpen = delayOpen;
        _delayTurn = delayTurn;
        _time = data.Time;
        _countShapes = data.CountShapes;
        _attempts = data.Count;
        _cardsArea.CreateCards(data.Size, _layers);
        _cardsArea.Shuffle();
    }

    public virtual IEnumerator StartRound_Coroutine(Queue<BonusTime> values, int countShuffle = 0)
    {
        SetupCards(values);

        yield return _cardsArea.Turn90Random(_delayOpen);
        yield return _timer.Run_Wait();
        yield return null;

        EventSetMaxAttempts?.Invoke(_attempts);
        EventSetTime.Invoke(_time);

        yield return _cardsArea.TurnToShirtRepeat(_delayOpen);
    }
    
    public void Run() => _cardsArea.ForEach((c) => c.IsInteractable = true);

    protected void AddTime(float add) => EventAddTime?.Invoke(_time += add);
    protected void LevelEnd() => EventEndLevel?.Invoke(_time);

    protected abstract void SetupCards(Queue<BonusTime> values);

    protected abstract void OnCardSelected(TimeCard card);
}
