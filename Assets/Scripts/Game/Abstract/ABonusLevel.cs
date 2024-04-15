using System;
using UnityEngine;

public abstract class ABonusLevel : MonoBehaviour
{
    [SerializeField] protected float _ratioTimeShow = 0.5f;

    protected TimeCardsArea _cardsArea;

    private int _attempts;
    protected float _delayOpen, _delayTurn;

    protected WaitForSeconds _waitShowEndLevel;

    protected int Attempts { get => _attempts;  set { _attempts = value; EventChangedAttempts?.Invoke(value); } }

    public Action<int> EventSelectedCard;
    public event Action<int> EventChangedAttempts;
    public Action EventEndLevel;

    public void Initialize(TimeCardsArea cardsArea, WaitForSeconds waitShowEndLevel)
    {
        _cardsArea = cardsArea;
        _waitShowEndLevel = waitShowEndLevel;
    }

    public virtual void Setup(int size, int attempts, float delayOpen, float delayTurn)
    {
        _delayOpen = delayOpen;
        _delayTurn = delayTurn;
        Attempts = attempts;
        _cardsArea.CreateCards(size, OnCardSelected);
        _cardsArea.Shuffle();
    }

    public void Play() => _cardsArea.ForEach((c) => c.InteractableOn());

    protected abstract void OnCardSelected(TimeCard card);
}
