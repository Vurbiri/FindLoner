using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardsArea), typeof(GridLayoutGroup))]
public class GameLevel : MonoBehaviour, ILevelPlay
{
    [SerializeField] private Sprite[] _shapeSprites;
    [Space]
    [SerializeField] private float _timeShow = 1f;
    [SerializeField] private float _timeShowGameOver = 2.5f;
    [Space]
    [SerializeField] private float _timeTurnPerAll = 2.5f;
    [Space]
    [SerializeField] private float _saturationMin = 0.275f;
    [SerializeField] private float _brightnessMin = 0.175f;

    private CardsArea _cardsArea;
    private GridLayoutGroup _thisGrid;
    private Vector2 _sizeArea, _defaultSpacing;

    private GameLevelSetupData _data;
    float _delayTurn;
    private WaitForSeconds _waitShow, _waitShowGameOver;

    private ShuffledArray<Sprite> _spritesRandom;

    public event Action EventStartLevel;
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

    public void StartLevel()
    {
        Setup(new());
        StartCoroutine(StartRound_Coroutine(true));
    }

    public void Play()
    {
        _cardsArea.ForEach((c) => c.IsInteractable = true);
        EventStartRound?.Invoke();
    }

    public void Setup(GameLevelSetupData data)
    {
        _data = data;
        _delayTurn = _timeTurnPerAll / data.CountShapes;
        int size = data.Size;

        _thisGrid.constraintCount = size;
        _thisGrid.cellSize = _sizeArea / size;
        _thisGrid.spacing = _defaultSpacing / (size - 1);

        _cardsArea.CreateCards(size, OnCardSelected);
        //StartCoroutine(StartRound_Coroutine(true));
    }

    private IEnumerator StartRound_Coroutine(bool isNew)
    {
        int[] groupsCard = CreateGroupsCard();
        _cardsArea.Shuffle();
        Stack<Shape> shapes = GetShapes();
        Vector3 axis = Direction2D.Random;
        Shape shape; Card card;

        for (int i = 0; i < groupsCard.Length; i++)
        {
            shape = shapes.Pop();

            for (int j = 0; j < groupsCard[i]; j++)
            {
                card = _cardsArea.RandomCard;
                if (isNew)
                    card.Setup(shape, _data.Size, axis, i);
                else
                    card.ReSetup(shape, axis, i);
            }
        }

        if (isNew)
        {
            yield return _cardsArea.Turn90Random(_delayTurn);
            Play(); //========= убрать потом
        }
        else
        {
            yield return _cardsArea.TurnRandom(_delayTurn);
            Play();
        }

        #region Local functions
        //==============================================================
        int[] CreateGroupsCard()
        {
            int[] groups = new int[_data.Count];
            int currentCount = _data.CountShapes - 1, countTypes = _data.Count - 1,
                avgSizeGroup = currentCount / countTypes,
                delta = avgSizeGroup - 2, add = 0, count;

            groups[0] = 1;
            for (int i = 1; i < countTypes; i++)
            {
                if (delta > 0)
                    add = add == 0 ? UnityEngine.Random.Range(-delta, delta + 1) : -add;

                count = avgSizeGroup + add;
                groups[i] = count;
                currentCount -= count;
            }
            groups[countTypes] = currentCount;
            
            return groups;
        }
        //-----
        Stack<Shape> GetShapes()
        {
            _spritesRandom.Shuffle();
            Stack<Shape> shs = new(_data.Count);
            Shape sh;
            Color color = Color.white; color.Randomize(_saturationMin, _brightnessMin);

            for (int i = 0; i < _data.Count; i++)
            {
                sh = new(_spritesRandom.Next, color);
                if (!_data.IsMonochrome)
                    sh.SetUniqueColor(shs, _saturationMin, _brightnessMin);
                shs.Push(sh);
            }

            return shs;
        }
        #endregion
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
            Setup(new());
            StartCoroutine(StartRound_Coroutine(true));
        }
        #endregion
    }
}
