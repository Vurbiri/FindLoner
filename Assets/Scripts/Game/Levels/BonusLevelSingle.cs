using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLevelSingle : ABonusLevel
{
    [SerializeField] private float _timeShuffle = 1.2f;
    [SerializeField] private float _delayShuffle = 0.3f;

    private WaitForSeconds _waitShuffle;

    private void Awake()
    {
        _waitShuffle = new(_delayShuffle);
    }

    public override IEnumerator StartRound_Coroutine(Queue<BonusTime> values, int countShuffle)
    {
        yield return StartCoroutine(base.StartRound_Coroutine(values));
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
                bonus = cards[3].Bonus;
                for (i = cards.Length - 2; i >= 0; i--)
                    waitAll.Add(cards[i].ReplaceCard_Coroutine(cards[i + 1], _timeShuffle));
                waitAll.Add(cards[3].ReplaceCard_Coroutine(cards[0], bonus, _timeShuffle));

                yield return waitAll;

                for (i = 0; i < cards.Length; i++)
                    cards[i].ResetPosition();

                countShuffle--;

                yield return _waitShuffle;
            }

            yield return null;
        }
        #endregion
    }

    protected override void SetupCards(Queue<BonusTime> values)
    {
        Vector3 axis = Direction2D.Random;
        BonusTime bonus = null;

        while (_cardsArea.TryGetRandomCard(out TimeCard card))
        {
            if (values.Count > 0)
                bonus = values.Dequeue();
            card.Setup(bonus, axis, OnCardSelected);
        }
    }

    protected override void OnCardSelected(TimeCard card)
    {
        bool continueLevel;
        //if(!(continueLevel = (card.Value > 0 || --Attempts > 0) && _countShapes > 0))
        if (!(continueLevel = --Attempts > 0 && _countShapes > 0))
            _cardsArea.ForEach((c) => c.IsInteractable = false);

        EventSelectedCard?.Invoke(card.Value);
        StartCoroutine(CardSelected_Coroutine());

        #region Local functions
        IEnumerator CardSelected_Coroutine()
        {
            yield return StartCoroutine(card.CardSelected_Coroutine());

            if (continueLevel) yield break;

            Attempts = 0;
            if (_countShapes > 0)
                yield return _cardsArea.TurnToValueRandom(_delayTurn);
            yield return _waitShowEndLevel;
            yield return _cardsArea.CardHideAndUnsubscribeRandom(_delayTurn / 2f);

            EventEndLevel?.Invoke();
        }
        #endregion
    }
}
