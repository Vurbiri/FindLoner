using System;
using UnityEngine;

public abstract class ABonusLevel : MonoBehaviour
{
    [SerializeField] protected float _ratioTimeShow = 0.5f;

    protected TimeCardsArea _cardsArea;

    private int _attempts;
    protected int _countShapes;
    protected float _delayOpen, _delayTurn;

    protected WaitForSeconds _waitShowEndLevel;

    protected int Attempts { get => _attempts;  set { _attempts = value; EventChangedAttempts?.Invoke(value); } }

    public Action<int> EventSelectedCard;
    public event Action<int> EventChangedAttempts;
    public event Action<int> EventChangedMaxAttempts;
    public Action EventEndLevel;

    public void Initialize(TimeCardsArea cardsArea, WaitForSeconds waitShowEndLevel)
    {
        _cardsArea = cardsArea;
        _waitShowEndLevel = waitShowEndLevel;
    }

    public void Setup(int size, int attempts, float delayOpen, float delayTurn)
    {
        _delayOpen = delayOpen;
        _delayTurn = delayTurn;
        _countShapes = size * size;
        EventChangedMaxAttempts?.Invoke(attempts);
        Attempts = attempts;
        _cardsArea.CreateCards(size, OnCardSelected);
        _cardsArea.Shuffle();
    }

    public void Play() => _cardsArea.ForEach((c) => c.InteractableOn());

    protected abstract void OnCardSelected(TimeCard card);
}
