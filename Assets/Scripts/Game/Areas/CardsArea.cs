using System;
using System.Collections;
using UnityEngine;

public class CardsArea : ACardsArea<Card, Action<Card>>
{
    public Coroutine TurnRandom(float delay) => TraversingRandom(delay, Turn);
    public Coroutine TurnRepeat(float delay) => TraversingRepeat(delay, Turn);

    private IEnumerator Turn(Card card) => card.Turn_Coroutine();

    protected override void AdditionalActionsCreatingCard(Card card, Action<Card> action)
    {
        card.EventSelected += action;
    }
}
