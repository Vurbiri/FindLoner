using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GameLevel : MonoBehaviour
{
    [SerializeField] private Card _prefabCard;
    [SerializeField] private Transform _repository;
    [Space]
    [SerializeField] private List<Sprite> _shapeSprites;
    [Space]
    [SerializeField] private float _timeShow = 1f;
    [Space]
    [SerializeField] private float _saturationMin = 0.16f;
    [SerializeField] private float _brightnessMin = 0.2f;

    private GridLayoutGroup _thisGrid;
    private Transform _thisTransform;
    private Vector2 _sizeArea, _defaultSpacing;

    private int _size, _countShapes, _countTypes;
    private bool _isMonochrome;
    private WaitForSeconds _waitShow;

    private readonly Stack<Card> _cardsActive = new();
    private readonly Stack<Card> _cardsRepository = new();
    private int[] _groupsCard;

    public event Action EventStartRound;
    public event Action<bool> EventEndRound;

    private void Awake()
    {
        _thisGrid = GetComponent<GridLayoutGroup>();
        _thisTransform = transform;
        _defaultSpacing = _thisGrid.spacing;
        _sizeArea = GetComponent<RectTransform>().rect.size - _defaultSpacing * 2;
        _waitShow = new(_timeShow);
    }

    private void Start()
    {
        LevelSetup(12, 5, false);
        StartCoroutine(StartRound(true));
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
        
        if (_cardsActive.Count == _countShapes) return;

        Card card;
        while (_cardsActive.Count > _countShapes) 
        {
            card = _cardsActive.Pop();
            card.Deactivate(_repository);
            _cardsRepository.Push(card);
        }

        while (_cardsActive.Count < _countShapes)
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

    private IEnumerator StartRound(bool isNew)
    {
        WaitAll waitAll = new(this);
        
        CreateGroupsCard();
        List<Shape> shapes = GetShapes();
        List<Card> cards = new(_cardsActive);
        Card card;
        Vector3 axis = Direction2D.Random;

        for (int i = 0; i < _groupsCard.Length; i++)
        {
            for (int j = 0; j < _groupsCard[i]; j++)
            {
                card = cards.RandomPop();
                if(isNew)
                    waitAll.Add(card.Setup(shapes[i], _size, axis, i));
                else
                    waitAll.Add(card.ReSetup(shapes[i], Direction2D.Random, i));
            }
        }

        yield return waitAll;
        
        EventStartRound?.Invoke();
    }

    private void OnCardSelected(int idGroup)
    {
        bool isContinue = idGroup == 0;
        
        EventEndRound?.Invoke(isContinue);
        foreach (var card in _cardsActive)
            card.CheckCroup(idGroup);

        if (isContinue)
            StartCoroutine(NextRound());

        #region Local function
        IEnumerator NextRound()
        {
            yield return _waitShow;
            StartCoroutine(StartRound(false));
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
        List<Shape> shapes = new(_countTypes);
        Shape shape;
        Color color = Color.white; color.Randomize(_saturationMin, _brightnessMin);

        for (int i = 0; i < _countTypes; i++)
        {
            shape = new(_shapeSprites.RandomPop(), color);
            if (!_isMonochrome)
                shape.SetUniqueColor(shapes, _saturationMin, _brightnessMin);
            shapes.Add(shape);
        }

        foreach (var s in shapes)
            _shapeSprites.Add(s.Sprite);

        return shapes;
    }
}
