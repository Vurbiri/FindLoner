using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardsArea), typeof(GridLayoutGroup))]
public class GameLevel : MonoBehaviour
{
    [SerializeField] private Sprite[] _shapeSprites;
    [Space]
    [SerializeField] private float _timeShow = 1f;
    [SerializeField] private float _timeShowGameOver = 2.5f;
    [Space]
    [SerializeField] private float _delayTurnPerAll = 2.5f;
    [Space]
    [SerializeField] private float _saturationMin = 0.275f;
    [SerializeField] private float _brightnessMin = 0.175f;

    private CardsArea _cardsArea;
    private GridLayoutGroup _thisGrid;
    private Vector2 _sizeArea, _defaultSpacing;

    private int _size, _countShapes, _countTypes;
    float _delayTurn;
    private bool _isMonochrome;
    private WaitForSeconds _waitShow, _waitShowGameOver;

    private ShuffledArray<Sprite> _spritesRandom;
    private int[] _groupsCard;

    public event Action EventStartRound;
    public event Action<bool> EventEndRound;
    public event Action<bool> EventEndLevel;

    private void Awake()
    {
        _cardsArea = GetComponent<CardsArea>();
        _thisGrid = GetComponent<GridLayoutGroup>();
        _defaultSpacing = _thisGrid.spacing;
        _sizeArea = GetComponent<RectTransform>().rect.size - _defaultSpacing * 2;
        _waitShow = new(_timeShow);
        _waitShowGameOver = new(_timeShowGameOver);
        _spritesRandom = new(_shapeSprites);
    }

    private void Start()
    {
        int size = UnityEngine.Random.Range(2, 12);
        Setup(size, size, false);
        StartCoroutine(StartRound_Coroutine(true));
    }

    public void Setup(int size, int countTypes, bool isMonochrome)
    {
        _size = size;
        _countShapes = size * size;
        _delayTurn = _delayTurnPerAll / _countShapes;
        _countTypes = countTypes;
        _isMonochrome = isMonochrome;

        _thisGrid.constraintCount = size;
        _thisGrid.cellSize = _sizeArea / size;
        _thisGrid.spacing = _defaultSpacing / (size - 1);

        _groupsCard = new int[_countTypes];

        _cardsArea.CreateCards(size, OnCardSelected);
    }

    public void Play() => _cardsArea.ForEach((c) => c.IsInteractable = true);

    private IEnumerator StartRound_Coroutine(bool isNew)
    {
        WaitAll waitAll = new(this);
        
        CreateGroupsCard();
        _cardsArea.Shuffle();

        Stack<Shape> shapes = GetShapes();
        Shape shape; Card card;
        Vector3 axis = Direction2D.Random;
        
        for (int i = 0; i < _groupsCard.Length; i++)
        {
            shape = shapes.Pop();

            for (int j = 0; j < _groupsCard[i]; j++)
            {
                card = _cardsArea.RandomCard;
                if (isNew)
                    card.Setup(shape, _size, axis, i);
                else
                    card.ReSetup(shape, axis, i);
            }
        }

        if (isNew)
            yield return _cardsArea.Turn90Random(_delayTurn);
        else
            yield return _cardsArea.TurnRandom(_delayTurn);

        Play(); //========= убрать потом

        EventStartRound?.Invoke();
    }

    private void OnCardSelected(Card card)
    {
        int id = card.IdGroup;
        bool isContinue = id == 0;
        
        EventEndRound?.Invoke(isContinue);
        _cardsArea.ForEach((c) => c.CheckCroup(id));

        if (isContinue)
            StartCoroutine(NextRound_Coroutine());
        else
            StartCoroutine(NewRound_Coroutine());

        #region Local function
        IEnumerator NextRound_Coroutine()
        {
            yield return _waitShow;
            StartCoroutine(StartRound_Coroutine(false));
        }
        IEnumerator NewRound_Coroutine()
        {
            yield return _waitShowGameOver;
            yield return _cardsArea.Turn90Random(_delayTurn / 2f);
            EventEndLevel?.Invoke(false);

            int size = UnityEngine.Random.Range(2, 12);
            Setup(size, size, false);
            StartCoroutine(StartRound_Coroutine(true));
        }
        #endregion
    }

    private void CreateGroupsCard()
    {
        int currentCount = _countShapes - 1, countTypes = _countTypes - 1,
            avgSizeGroup = currentCount / countTypes,
            delta = avgSizeGroup - 2, add = 0, count;

        _groupsCard[0] = 1;
        for (int i = 1;  i < countTypes; i++) 
        {
            if (delta > 0)
                add = add == 0 ? UnityEngine.Random.Range(-delta, delta + 1) : -add;

            count = avgSizeGroup + add;
            _groupsCard[i] = count;
            currentCount -= count;
        }
        _groupsCard[countTypes] = currentCount;
    }

    private Stack<Shape> GetShapes() 
    {
        _spritesRandom.Shuffle();
        Stack<Shape> shapes = new(_countTypes);
        Shape shape;
        Color color = Color.white; color.Randomize(_saturationMin, _brightnessMin);

        for (int i = 0; i < _countTypes; i++)
        {
            shape = new(_spritesRandom.Next, color);
            if (!_isMonochrome)
                shape.SetUniqueColor(shapes, _saturationMin, _brightnessMin);
            shapes.Push(shape);
        }

        return shapes;
    }
}
