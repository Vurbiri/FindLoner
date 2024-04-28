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
        _sound.PlayStart();

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
                _sound.PlayShuffle();
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
        card.Fixed(); _countShapes--;
        bool continueLevel = --Attempts > 0 && _countShapes > 0;
        //bool continueLevel = (card.Value > 0 || --Attempts > 0) && _countShapes > 0;

        if (continueLevel)
            foreach (var c in _cardsArea)
                if (continueLevel = c.IsValue) break;

        if (!continueLevel)
            _cardsArea.ForEach((c) => c.IsInteractable = false);


        _sound.PlayTurn();
        
        StartCoroutine(CardSelected_Coroutine());

        #region Local functions
        IEnumerator CardSelected_Coroutine()
        {
            yield return StartCoroutine(card.CardSelected_Coroutine());

            if (card.Value > 0)  { AddTime(card.Value); _sound.PlayFixed(); card.SetColorTrue(); }
            else { _sound.PlayError(); card.SetColorError(); }

            if (continueLevel) yield break;

            Attempts = 0;

            if (_countShapes > 0)
                yield return _cardsArea.TurnToValueRandom(_delayTurn);
            yield return _waitShowEndLevel;
            yield return _cardsArea.CardHideAndUnsubscribeRandom(_delayTurn / 2f);

            LevelEnd();
        }
        #endregion
    }
}
