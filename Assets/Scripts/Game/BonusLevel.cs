using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusLevel : MonoBehaviour
{
    [SerializeField] private TimeCard _prefabCard;
    [SerializeField] private Transform _repository;
    [Space]
    [SerializeField] private float _startTimeShow = 1f;

    private GridLayoutGroup _thisGrid;
    private Transform _thisTransform;
    private Vector2 _sizeArea, _defaultSpacing;

    private WaitForSeconds _waitShow;

    private readonly Stack<TimeCard> _cardsActive = new();
    private readonly Stack<TimeCard> _cardsRepository = new();

    public event Action EventStartRound;
    public event Action<bool> EventEndRound;

    private void Awake()
    {
        _thisGrid = GetComponent<GridLayoutGroup>();
        _thisTransform = transform;
        _defaultSpacing = _thisGrid.spacing;
        _sizeArea = GetComponent<RectTransform>().rect.size - _defaultSpacing * 2;
    }

    private void Start()
    {
        LevelSetup(8, false);
    }

    private void LevelSetup(int size, bool isMonochrome)
    {
        int countShapes = size * size;
        Vector2 cellSize = _sizeArea / size;

        _waitShow = new(_startTimeShow + size * 0.1f);

        _thisGrid.constraintCount = size;
        _thisGrid.cellSize = cellSize;
        _thisGrid.spacing = _defaultSpacing / (size - 1);

        CreateCards();
        StartCoroutine(StartRound(new(0,15)));

        #region Local functions
        void CreateCards()
        {
            if (_cardsActive.Count == countShapes) return;

            TimeCard card;
            while (_cardsActive.Count > countShapes)
            {
                card = _cardsActive.Pop();
                card.Deactivate(_repository);
                _cardsRepository.Push(card);
            }

            while (_cardsActive.Count < countShapes)
            {
                if (_cardsRepository.Count > 0)
                {
                    card = _cardsRepository.Pop();
                    card.Activate(_thisTransform);
                }
                else
                {
                    card = Instantiate(_prefabCard, _thisTransform);
                    card.EventSelected += OnCardSelected;
                }

                _cardsActive.Push(card);
            }
        }
        IEnumerator StartRound(Vector2Int range)
        {
            WaitAll waitAll = new(this);

            List<TimeCard> cards = new(_cardsActive);
            //TimeCard card;
            Vector3 axis = Direction2D.Random;
            int index = 0;

            foreach (var card in _cardsActive)
                waitAll.Add(card.Setup(index++, cellSize.x, size, axis, 0));

            //while (cards.Count > 0)
            //{
            //    card = cards.RandomPop();
            //    waitAll.Add(card.Setup(index++, cellSize.x, size, axis, 0));
            //}

            yield return waitAll;
            yield return _waitShow;

            //foreach (var item in _cardsActive)
            //    waitAll.Add(item.Hide());
            //yield return waitAll;

            EventStartRound?.Invoke();
        }
        #endregion
    }

    private void OnCardSelected(TimeCard card)
    {
        StartCoroutine(card.Show());
    }
}
