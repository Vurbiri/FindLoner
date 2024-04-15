using System;
using System.Collections;
using UnityEngine;

public class BonusLevelPair : ABonusLevel
{
    [SerializeField] private float _timeShowPair = 0.75f;

    private int _countShapes;
    private TimeCard _cardSelect;
    private WaitForSeconds _waitShowPair;

    private void Awake()
    {
        _waitShowPair = new(_timeShowPair);
    }

    public override void Setup(int size, int attempts, float delayOpen, float delayTurn)
    {
        base.Setup(size, attempts, delayOpen, delayTurn);
        _countShapes = size * size;
    }

    public IEnumerator StartRound_Coroutine(int size, float cellSize, Increment range)
    {
        Vector3 axis = Direction2D.Random;

        CardsSetup();

        yield return _cardsArea.ShowRandom(_delayOpen);
        yield return new WaitForSeconds(size * _ratioTimeShow);

        yield return _cardsArea.TurnToShirtRepeat(_delayOpen);

        Play(); //========= убрать потом

        #region Local function
        void CardsSetup()
        {
            TimeCard card;
            int value = -1; 
            Action setup = Setup;
            
            _cardSelect = null;
            if (_countShapes % 2 != 0)
            {
                _cardSelect = _cardsArea.CardCenter;
                _cardSelect.Setup(-1, cellSize, size, axis, true);
                setup = SetupNotCardCenter;
                _countShapes--;
            }

            int count = _countShapes / 2;
            while (count > 0)
            {
                value = range.Next;
                setup();
                setup();
                count--;
            }

            #region Local function
            void Setup()
            {
                card = _cardsArea.RandomCard;
                card.Setup(value, cellSize, size, axis);
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
                card.Setup(value, cellSize, size, axis);
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
            Attempts--;
            isOne = false;
            if (!(isClose = _cardSelect.Value != card.Value))
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
                yield return _cardsArea.TurnToValueRandom(_delayTurn);
                yield return _waitShowEndLevel;

                EventEndLevel?.Invoke();
            }
        }
        #endregion
    }
}
