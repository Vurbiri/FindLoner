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
    [SerializeField] private float _delayTurn = 0.025f;
    [Space]
    [SerializeField] private float _saturationMin = 0.275f;
    [SerializeField] private float _brightnessMin = 0.175f;

    private CardsArea _cardsArea;
    private GridLayoutGroup _thisGrid;
    private Vector2 _sizeArea, _defaultSpacing;

    private int _size, _countShapes, _countTypes;
    private bool _isMonochrome;
    private WaitForSeconds _waitShow;

    private ShuffledArray<Sprite> _spritesRandom;
    private int[] _groupsCard;

    public event Action EventStartRound;
    public event Action<bool> EventEndRound;

    private void Awake()
    {
        _cardsArea = GetComponent<CardsArea>();
        _thisGrid = GetComponent<GridLayoutGroup>();
        _defaultSpacing = _thisGrid.spacing;
        _sizeArea = GetComponent<RectTransform>().rect.size - _defaultSpacing * 2;
        _waitShow = new(_timeShow);
        _spritesRandom = new(_shapeSprites);
    }

    private void Start()
    {
        int size = UnityEngine.Random.Range(2, 12);
        LevelSetup(size, size, false);
        StartCoroutine(StartRound_Coroutine(true));
    }

    private void LevelSetup(int size, int countTypes, bool isMonochrome)
    {
        _size = size;
        _countShapes = size * size;
        _countTypes = countTypes;
        _isMonochrome = isMonochrome;

        _thisGrid.constraintCount = size;
        _thisGrid.cellSize = _sizeArea / size;
        _thisGrid.spacing = _defaultSpacing / (size - 1);

        _groupsCard = new int[_countTypes];

        _cardsArea.CreateCards(size, OnCardSelected);
    }

    private IEnumerator StartRound_Coroutine(bool isNew)
    {
        WaitAll waitAll = new(this);
        
        CreateGroupsCard();
        _cardsArea.Shuffle();
        List<Shape> shapes = GetShapes();
        Vector3 axis = Direction2D.Random;
        Card card;
        for (int i = 0; i < _groupsCard.Length; i++)
        {
            for (int j = 0; j < _groupsCard[i]; j++)
            {
                card = _cardsArea.RandomCard;
                if (isNew)
                    card.Setup(shapes[i], _size, axis, i);
                else
                    card.ReSetup(shapes[i], axis, i);
            }
        }

        if (isNew)
            yield return _cardsArea.ShowRandom(_delayTurn);
        else
            yield return _cardsArea.TurnRandom(_delayTurn);

        EventStartRound?.Invoke();
    }

    private void OnCardSelected(ACard card)
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
            yield return new WaitForSeconds(2.5f);
            int size = UnityEngine.Random.Range(2, 12);
            LevelSetup(size, size, false);
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

    private List<Shape> GetShapes() 
    {
        _spritesRandom.Shuffle();
        List<Shape> shapes = new(_countTypes);
        Shape shape;
        Color color = Color.white; color.Randomize(_saturationMin, _brightnessMin);

        for (int i = 0; i < _countTypes; i++)
        {
            shape = new(_spritesRandom.Next, color);
            if (!_isMonochrome)
                shape.SetUniqueColor(shapes, _saturationMin, _brightnessMin);
            shapes.Add(shape);
        }

        return shapes;
    }
}
