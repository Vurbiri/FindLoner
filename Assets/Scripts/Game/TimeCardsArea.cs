using System.Collections;
using UnityEngine;

public class TimeCardsArea : ACardsArea<TimeCard>
{
    private const int COUNT_SQUAD = 4;

    public TimeCard[] RandomSquadCards
    { 
        get
        {
            TimeCard[] cards = new TimeCard[COUNT_SQUAD];
            Vector2Int index = new(Random.Range(1, _sizeArea - 1), Random.Range(1, _sizeArea - 1));
            Vector2Int[] around = Direction2D.RandomAround;
            cards[0] = _cardsActive[index];
            for (int i = 1; i < COUNT_SQUAD; i++)
                cards[i] = _cardsActive[index += around[i - 1]];

            return cards;
        }
    }

    public TimeCard CardCenter => _cardsActive[_sizeArea / 2, _sizeArea / 2];


    public Coroutine TurnToValueRandom(float delay) => TraversingRandom(delay, TurnToValue);
    public Coroutine TurnToValueRepeat(float delay) => TraversingRepeat(delay, TurnToValue);

    public Coroutine TurnToShirtRandom(float delay) => TraversingRandom(delay, TurnToShirt);
    public Coroutine TurnToShirtRepeat(float delay) => TraversingRepeat(delay, TurnToShirt);

    private IEnumerator TurnToValue(TimeCard card) => card.TurnToValue_Coroutine();
    private IEnumerator TurnToShirt(TimeCard card) => card.TurnToShirt_Coroutine();

}
