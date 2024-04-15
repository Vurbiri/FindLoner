using System.Collections;
using UnityEngine;


public class BonusLevelSingle : ABonusLevel
{
    [SerializeField] private float _timeShuffle = 1.5f;

    public IEnumerator StartRound_Coroutine(int size, float cellSize, Increment range, int countShuffle)
    {
        Vector3 axis = Direction2D.Random;

        while (_cardsArea.TryGetRandomCard(out TimeCard card))
            card.Setup(range.Next, cellSize, size, axis);

        yield return _cardsArea.ShowRandom(_delayOpen);
        yield return new WaitForSeconds(size * _ratioTimeShow);

        yield return _cardsArea.TurnToShirtRepeat(_delayOpen);

        yield return StartCoroutine(Shuffle_Coroutine());

        Play(); //========= убрать потом

        #region Local function
        IEnumerator Shuffle_Coroutine()
        {
            if (countShuffle <= 0)
                yield break;

            TimeCard[] cards;
            WaitAll waitAll = new(this);
            int i, tempValue;
            while (countShuffle > 0)
            {
                cards = _cardsArea.RandomSquadCards;
                tempValue = cards[0].Value;

                for (i = 0; i < cards.Length - 1; i++)
                    waitAll.Add(cards[i].ReplaceCard_Coroutine(cards[i + 1], _timeShuffle));
                waitAll.Add(cards[3].ReplaceCard_Coroutine(cards[0], tempValue, _timeShuffle));

                yield return waitAll;

                for (i = 0; i < cards.Length; i++)
                    cards[i].ResetPosition();

                countShuffle--;
            }

            yield return null;
        }
        #endregion
    }

    protected override void OnCardSelected(TimeCard card)
    {
        bool endLevel;
        if(endLevel = --Attempts == 0)
            _cardsArea.ForEach((c) => c.InteractableOff());

        EventSelectedCard?.Invoke(card.Value);
        StartCoroutine(CardSelected_Coroutine());

        #region Local functions
        IEnumerator CardSelected_Coroutine()
        {
            yield return StartCoroutine(card.CardSelected_Coroutine());

            if (!endLevel) yield break;

            yield return _cardsArea.TurnToValueRandom(_delayTurn);
            yield return _waitShowEndLevel;

            EventEndLevel?.Invoke();
        }
        #endregion
    }
}
