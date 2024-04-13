using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TimeCardsArea), typeof(GridLayoutGroup))]
public class BonusLevel : MonoBehaviour
{
    [SerializeField] private float _startTimeShow = 1f;
    [SerializeField] private float _delayOpen = 0.015f;
    [SerializeField] private float _delayTurn = 0.025f;

    private TimeCardsArea _cardsArea;
    private GridLayoutGroup _thisGrid;
    private Vector2 _sizeArea, _defaultSpacing;

    private WaitForSeconds _waitShow;
   
    private int _attempts;

    public event Action EventStartLevel;
    public event Action<int> EventSelectedCard;
    public event Action EventEndLevel;

    private void Awake()
    {
        _cardsArea = GetComponent<TimeCardsArea>();
        _thisGrid = GetComponent<GridLayoutGroup>();
        _defaultSpacing = _thisGrid.spacing;
        _sizeArea = GetComponent<RectTransform>().rect.size - _defaultSpacing * 2;
    }

    private void Start()
    {
        LevelSetup(3, 6, new(0));
    }

    private void LevelSetup(int attempts, int size, Increment range)
    {
        _attempts = attempts;
        int countShapes = size * size;
        Vector2 cellSize = _sizeArea / size;

        _waitShow = new(_startTimeShow + size * 0.1f);

        _thisGrid.constraintCount = size;
        _thisGrid.cellSize = cellSize;
        _thisGrid.spacing = _defaultSpacing / (size - 1);

        _cardsArea.CreateCards(size, OnCardSelected);

        StartCoroutine(StartRound_Coroutine(range));

        #region Local functions
        IEnumerator StartRound_Coroutine(Increment range)
        {
            _cardsArea.Shuffle();
            Vector3 axis = Direction2D.Random;

            while (_cardsArea.TryGetRandomCard(out TimeCard card))
                card.Setup(range.Next, cellSize.x, size, axis, 0);

            yield return _cardsArea.ShowRandom(_delayOpen);
            yield return _waitShow;

            yield return _cardsArea.TurnRepeat(_delayOpen);

            EventStartLevel?.Invoke();
        }
        #endregion
    }

    private void OnCardSelected(ACard aCard)
    {
        TimeCard card = (TimeCard)aCard;
        bool endLevel = --_attempts == 0;

        EventSelectedCard?.Invoke(card.Value);
        StartCoroutine(CardSelected_Coroutine());

        #region Local functions
        IEnumerator CardSelected_Coroutine()
        {
            if (endLevel)
                _cardsArea.ForEach((c) => c.IsInteractable = false);

            yield return StartCoroutine(card.TurnToValue_Coroutine());

            if (!endLevel) yield break;

            yield return _cardsArea.TurnToValueRandom(_delayTurn);

            EventEndLevel?.Invoke();

            yield return new WaitForSeconds(1.5f);
            LevelSetup(3, UnityEngine.Random.Range(3, 13), new(0));
        }
        #endregion
    }


}
