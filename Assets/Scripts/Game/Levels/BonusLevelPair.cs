using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLevelPair : ABonusLevel
{
    [SerializeField] private float _timeShowPair = 1.25f;

    private TimeCard _cardSelect;
    private WaitForSeconds _waitShowPair;

    private void Awake()
    {
        _waitShowPair = new(_timeShowPair);
    }

    protected override void SetupCards(Queue<BonusTime> values)
    {
        _cardSelect = null;
        Vector3 axis = Direction2D.Random;
        BonusTime bonus = null;
        TimeCard card, cardCenter = null;
        Action<BonusTime> setup = Setup;

        if (_countShapes % 2 != 0)
        {
            cardCenter = _cardsArea.CardCenter;
            cardCenter.Setup(null, axis, null, true);
            setup = SetupNotCardCenter;
            _countShapes--;
        }

        int count = _countShapes / 2;
        while (count > 0)
        {
            if (values.Count > 0)
                bonus = values.Dequeue();
            setup(bonus);
            setup(bonus);
            count--;
        }

        #region Local function
        void Setup(BonusTime bonusTime)
        {
            card = _cardsArea.RandomCard;
            card.Setup(bonusTime, axis, OnCardSelected);
        }
        void SetupNotCardCenter(BonusTime bonusTime)
        {
            card = _cardsArea.RandomCard;
            if (card == cardCenter)
            {
                card = _cardsArea.RandomCard;
                setup = Setup;
                cardCenter = null;
            }
            card.Setup(bonusTime, axis, OnCardSelected);
        }
        #endregion
    }

    protected override void OnCardSelected(TimeCard card)
    {
        bool isClose = false, isOne = true, isContinue = true;

        if (_cardSelect != null)
        {
            _cardsArea.ForEach((c) => c.IsInteractable = false);
            isOne = false;

            Attempts--;
            if (!(isClose = _cardSelect.Value != card.Value))
                AddTime(card.Value);
            //if (isClose = _cardSelect.Value != card.Value)
            //    Attempts--;
            //else
            //    AddTime(card.Value);
        }
        else
        {
            _cardSelect = card;
        }

        _sound.PlayTurn();
        StartCoroutine(CardSelected_Coroutine());

        #region Local functions
        //=========================================
        IEnumerator CardSelected_Coroutine()
        {
            yield return StartCoroutine(card.CardSelected_Coroutine());

            if (isOne)
                yield break;

            if (isClose)
            {
                _sound.PlayError();
                card.SetColorError();
                _cardSelect.SetColorError();
                if (Attempts > 0)
                {
                    yield return _waitShowPair;
                    yield return new WaitAll(this, card.CardClose_Coroutine(), _cardSelect.CardClose_Coroutine());
                }
                else
                {
                    _countShapes -= 2;
                }
            }
            else
            {
                _sound.PlayFixed();
                card.FixedAndSetColorTrue();
                _cardSelect.FixedAndSetColorTrue();

                if(isContinue = (_countShapes -= 2) > 0)
                    foreach (var c in _cardsArea)
                        if(isContinue = c.IsValue)
                            break;
            }

            _cardSelect = null;

            if (isContinue && Attempts > 0)
            {
                _cardsArea.ForEach((c) => c.IsInteractable = true);
            }
            else
            {
                Attempts = 0;
                if (_countShapes > 0)
                    yield return _cardsArea.TurnToValueRandom(_delayTurn);
                yield return _waitShowEndLevel;
                yield return _cardsArea.CardHideAndUnsubscribeRandom(_delayTurn / 2f);

                LevelEnd();
            }
        }
        #endregion
    }
}
