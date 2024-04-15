using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TimeCardsArea), typeof(GridLayoutGroup))]
public class BonusLevel : MonoBehaviour
{
    [SerializeField] private float _ratioTimeShow = 0.5f;
    [SerializeField] private float _timeShowPair = 0.75f;
    [SerializeField] private float _timeShuffle = 1.5f;
    [SerializeField] private float _delayOpenPerAll = 1.5f;
    [SerializeField] private float _delayTurnPerAll = 2.5f;

    private TimeCardsArea _cardsArea;
    private GridLayoutGroup _thisGrid;
    private Vector2 _sizeArea, _defaultSpacing;

    private WaitForSeconds _waitShowPair;

    private int _size, _countShapes, _attempts;
    float _delayOpen, _delayTurn;

    TimeCard _cardSelect;

    public event Action EventStartLevel;
    public event Action<int> EventSelectedCard;
    public event Action EventEndLevel;

    private void Awake()
    {
        _cardsArea = GetComponent<TimeCardsArea>();
        _thisGrid = GetComponent<GridLayoutGroup>();
        _defaultSpacing = _thisGrid.spacing;
        _sizeArea = GetComponent<RectTransform>().rect.size - _defaultSpacing * 2;
        _waitShowPair = new(_timeShowPair);
    }

    private void Start()
    {
        LevelSetup(5, new(0), 0, BonusLevelTypes.Pair, 3);
    }

    private void LevelSetup(int size, Increment range, int countShuffle, BonusLevelTypes type, int attempts = 0)
    {
        _countShapes = size * size;
        _delayOpen = _delayOpenPerAll / _countShapes; _delayTurn = _delayTurnPerAll / _countShapes;
        Vector2 cellSize = _sizeArea / size;

        _thisGrid.constraintCount = size;
        _thisGrid.cellSize = cellSize;
        _thisGrid.spacing = _defaultSpacing / (size - 1);

        StartCoroutine(StartRound_Coroutine());

        #region Local function
        IEnumerator StartRound_Coroutine()
        {
            if (type == BonusLevelTypes.Single)
            {
                _attempts = attempts;
                _cardsArea.CreateCards(size, OnCardSelectedSingle);
                CardsSetupSingle(range, cellSize.x, size, Direction2D.Random);
            }
            else
            {
                _cardsArea.CreateCards(size, OnCardSelectedPair);
                CardsSetupPair(range, cellSize.x, size, Direction2D.Random);
            }

            yield return _cardsArea.ShowRandom(_delayOpen);
            yield return new WaitForSeconds(size * _ratioTimeShow);

            yield return _cardsArea.TurnToShirtRepeat(_delayOpen);

            yield return StartCoroutine(Shuffle_Coroutine(countShuffle));

            _cardsArea.ForEach((c) => c.InteractableOn());

            EventStartLevel?.Invoke();
        }
        #endregion
    }

    private void CardsSetupSingle(Increment range, float cellSize, int size, Vector3 axis)
    {
        _cardsArea.Shuffle();

        while (_cardsArea.TryGetRandomCard(out TimeCard card))
            card.Setup(range.Next, cellSize, size, axis);
    }

    private void CardsSetupPair(Increment range, float cellSize, int size, Vector3 axis)
    {
        _cardsArea.Shuffle();

        TimeCard card;
        int value = -1, count = _countShapes / 2;
        Action setup = Setup;
        _attempts = _countShapes;
        _cardSelect = null;
        if (_countShapes % 2 != 0)
        {
            _cardSelect = _cardsArea.CardCenter;
            _cardSelect.Setup(-1, cellSize, size, axis, true);
            setup = SetupNotCardCenter;
            _attempts--;
        }

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
            if(card == _cardSelect)
            {
                card = _cardsArea.RandomCard;
                setup = Setup;
                _cardSelect = null;
            }
            card.Setup(value, cellSize, size, axis);
        }
        #endregion
    }

    private void OnCardSelectedSingle(TimeCard card)
    {
        bool endLevel = --_attempts == 0;

        EventSelectedCard?.Invoke(card.Value);
        StartCoroutine(CardSelected_Coroutine());

        #region Local functions
        IEnumerator CardSelected_Coroutine()
        {
            if (endLevel)
                _cardsArea.ForEach((c) => c.InteractableOff());

            yield return StartCoroutine(card.CardSelected_Coroutine());

            if (!endLevel) yield break;

            yield return _cardsArea.TurnToValueRandom(_delayTurn);

            EventEndLevel?.Invoke();

            yield return new WaitForSeconds(1.5f);
            LevelSetup(UnityEngine.Random.Range(4, 13), new(0), UnityEngine.Random.Range(0, 21), BonusLevelTypes.Single, 3);
        }
        #endregion
    }

    private void OnCardSelectedPair(TimeCard card)
    {
        bool isClose = false, isEquity = false;

        _cardsArea.ForEach((c) => c.InteractableOff());
        if (_cardSelect != null)
            isClose = !(isEquity = _cardSelect.Value == card.Value);
        else
            _cardSelect = card;

        if (isEquity)
        {
            EventSelectedCard?.Invoke(card.Value);
            card.Disable();
            _cardSelect.Disable();
            _attempts -= 2;
            _cardSelect = null;
        }

        StartCoroutine(CardSelected_Coroutine());

        #region Local functions
        IEnumerator CardSelected_Coroutine()
        {

            yield return StartCoroutine(card.CardSelected_Coroutine());

            if(isClose)
            {
                yield return _waitShowPair;
                yield return new WaitAll(this, card.CardClose_Coroutine(), _cardSelect.CardClose_Coroutine());
                _cardSelect = null;
            }

            if (_attempts > 0)
            {
                _cardsArea.ForEach((c) => c.InteractableOn());
            }
            else
            {
                EventEndLevel?.Invoke();

                yield return new WaitForSeconds(1.5f);
                LevelSetup(UnityEngine.Random.Range(4, 13), new(0), UnityEngine.Random.Range(0, 3), BonusLevelTypes.Pair, 3);
            }
        }
        #endregion
    }



    IEnumerator Shuffle_Coroutine(int countShuffle)
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
}
