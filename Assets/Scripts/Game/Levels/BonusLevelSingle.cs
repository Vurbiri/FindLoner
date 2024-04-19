using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLevelSingle : ABonusLevel
{
    [SerializeField] private float _timeShuffle = 1.5f;

    public override IEnumerator StartRound_Coroutine(int size, float cellSize, Queue<BonusTime> values, int countShuffle)
    {
        yield return StartCoroutine(base.StartRound_Coroutine(size, cellSize, values));
        yield return StartCoroutine(Shuffle_Coroutine());

        #region Local function
        //=========================================================
        IEnumerator Shuffle_Coroutine()
        {
            if (countShuffle <= 0)
                yield break;

            BonusTime bonus;
            TimeCard[] cards;
            WaitAll waitAll = new(this);
            int i;
            while (countShuffle > 0)
            {
                cards = _cardsArea.RandomSquadCards;
                bonus = cards[0].Bonus;

                for (i = 0; i < cards.Length - 1; i++)
                    waitAll.Add(cards[i].ReplaceCard_Coroutine(cards[i + 1], _timeShuffle));
                waitAll.Add(cards[3].ReplaceCard_Coroutine(cards[0], bonus, _timeShuffle));

                yield return waitAll;

                for (i = 0; i < cards.Length; i++)
                    cards[i].ResetPosition();

                countShuffle--;
            }

            yield return null;
        }
        #endregion
    }

    protected override void SetupCards(int size, float cellSize, Queue<BonusTime> values)
    {
        Vector3 axis = Direction2D.Random;
        BonusTime bonus = null;

        while (_cardsArea.TryGetRandomCard(out TimeCard card))
        {
            if (values.Count > 0)
                bonus = values.Dequeue();
            card.Setup(bonus, cellSize, size, axis);
        }
    }

    protected override void OnCardSelected(TimeCard card)
    {
        bool continueLevel;
        if(!(continueLevel = (card.Value > 0 || --Attempts > 0) && _countShapes > 0))
            _cardsArea.ForEach((c) => c.InteractableOff());

        EventSelectedCard?.Invoke(card.Value);
        StartCoroutine(CardSelected_Coroutine());

        #region Local functions
        IEnumerator CardSelected_Coroutine()
        {
            yield return StartCoroutine(card.CardSelected_Coroutine());

            if (continueLevel) yield break;

            if (_countShapes > 0)
                yield return _cardsArea.TurnToValueRandom(_delayTurn);
            yield return _waitShowEndLevel;
            yield return _cardsArea.Turn90Random(_delayTurn / 2f);

            EventEndLevel?.Invoke();
            gameObject.SetActive(false);
        }
        #endregion
    }
}
