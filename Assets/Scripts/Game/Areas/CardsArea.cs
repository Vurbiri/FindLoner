
using System.Collections;
using UnityEngine;

public class CardsArea : ACardsArea<Card>
{
    public Coroutine TurnRandom(float delay) => TraversingRandom(delay, Turn);
    public Coroutine TurnRepeat(float delay) => TraversingRepeat(delay, Turn);

    private IEnumerator Turn(Card card) => card.Turn_Coroutine();
}
