using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABonusLevel : MonoBehaviour, ILevelPlay
{
    [SerializeField] protected float _ratioTimeShow = 0.1f;

    protected TimeCardsArea _cardsArea;

    private int _attempts;
    protected int _countShapes;
    protected float _delayOpen, _delayTurn;

    protected WaitForSeconds _waitShowEndLevel;

    protected int Attempts { get => _attempts;  set { _attempts = value; EventChangedAttempts?.Invoke(value); } }

    public Action<float> EventSelectedCard;
    public event Action<int> EventChangedAttempts;
    public Action EventEndLevel;

    public void Initialize(TimeCardsArea cardsArea, WaitForSeconds waitShowEndLevel)
    {
        _cardsArea = cardsArea;
        _waitShowEndLevel = waitShowEndLevel;
    }

    public virtual IEnumerator StartRound_Coroutine(int size, float cellSize, Queue<BonusTime> values, int countShuffle = 0)
    {
        SetupCards(size, cellSize, values);

        yield return _cardsArea.Turn90Random(_delayOpen);
        yield return new WaitForSeconds(_countShapes * _ratioTimeShow);

        yield return _cardsArea.TurnToShirtRepeat(_delayOpen);
    }

    public void Setup(LevelSetupData data, float delayOpen, float delayTurn)
    {
        _delayOpen = delayOpen;
        _delayTurn = delayTurn;
        _countShapes = data.CountShapes;
        Attempts = data.Count;
        _cardsArea.CreateCards(data.Size);
        _cardsArea.Shuffle();
    }

    public void Run() => _cardsArea.ForEach((c) => c.raycastTarget = true);

    protected abstract void SetupCards(int size, float cellSize, Queue<BonusTime> values);

    protected abstract void OnCardSelected(TimeCard card);
}
