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

    public IEnumerator StartRound_Coroutine(int size, float cellSize, Queue<BonusTime> values)
    {
        Vector3 axis = Direction2D.Random;

        CardsSetup();

        yield return _cardsArea.Turn90Random(_delayOpen);
        yield return new WaitForSeconds(size * _ratioTimeShow);

        yield return _cardsArea.TurnToShirtRepeat(_delayOpen);

        Play(); //========= ������ �����

        #region Local function
        void CardsSetup()
        {
            TimeCard card;
            BonusTime bonus = null;
            Action setup = Setup;
            
            _cardSelect = null;
            if (_countShapes % 2 != 0)
            {
                _cardSelect = _cardsArea.CardCenter;
                _cardSelect.Setup(null, cellSize, size, axis, true);
                setup = SetupNotCardCenter;
                _countShapes--;
            }

            int count = _countShapes / 2;
            while (count > 0)
            {
                if (values.Count > 0)
                    bonus = values.Dequeue();
                setup();
                setup();
                count--;
            }

            #region Local function
            void Setup()
            {
                card = _cardsArea.RandomCard;
                card.Setup(bonus, cellSize, size, axis);
            }
            void SetupNotCardCenter()
            {
                card = _cardsArea.RandomCard;
                if (card == _cardSelect)
                {
                    card = _cardsArea.RandomCard;
                    setup = Setup;
                    _cardSelect = null;
                }
                card.Setup(bonus, cellSize, size, axis);
            }
            #endregion
        }
        #endregion
    }

    protected override void OnCardSelected(TimeCard card)
    {
        bool isClose = false, isOne = true;
        
        if (_cardSelect != null)
        {
            _cardsArea.ForEach((c) => c.InteractableOff());
            isOne = false;
            if (isClose = _cardSelect.Value != card.Value)
                Attempts--;
            else
                EventSelectedCard?.Invoke(card.Value);
        }
        else
        {
            _cardSelect = card;
        }

        StartCoroutine(CardSelected_Coroutine());

        #region Local functions
        IEnumerator CardSelected_Coroutine()
        {
            yield return StartCoroutine(card.CardSelected_Coroutine());

            if (isOne)
                yield break;

            if (isClose)
            {
                card.SetColorError();
                _cardSelect.SetColorError();
                yield return _waitShowPair;
                yield return new WaitAll(this, card.CardClose_Coroutine(), _cardSelect.CardClose_Coroutine());
            }
            else
            {
                card.Fixed();
                _cardSelect.Fixed();
                _countShapes -= 2;
            }

            _cardSelect = null;

            if (Attempts > 0 && _countShapes > 0)
            {
                _cardsArea.ForEach((c) => c.InteractableOn());
            }
            else
            {
                if (_countShapes > 0)
                    yield return _cardsArea.TurnToValueRandom(_delayTurn);
                yield return _waitShowEndLevel;
                yield return _cardsArea.Turn90Random(_delayTurn / 2f);

                EventEndLevel?.Invoke();
            }
        }
        #endregion
    }
}