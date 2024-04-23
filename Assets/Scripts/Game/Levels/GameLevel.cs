using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(CardsArea))]
public class GameLevel : MonoBehaviour
{
    [SerializeField] private CreatorShapes _creatorShapes;
    [Space]
    [SerializeField] private float _timeShowEndRound = 1f;
    [SerializeField] private float _timeShowEndLevel = 2.5f;
    [Space]
    [SerializeField] private float _timeTurnPerAll = 2.5f;
    [Space]
    [SerializeField] private float _saturationMin = 0.275f;
    [SerializeField] private float _brightnessMin = 0.3f;
    [Space]
    [SerializeField] private bool _isCheats = true;

    private CardsArea _cardsArea;
    private bool _isFind = false;

    private LevelSetupData _data;
    float _delayTurn;
    private WaitForSeconds _waitShowEndRound, _waitShowEndLevel;
    //private Coroutine _coroutineNextRound, _coroutineEndLevel;

    public event Action EventStartLevel;
    public event Action EventStartRound;
    public event Action<bool> EventEndRound;
    public event Action<bool> EventEndLevel;

    public void Initialize(float sizeArea, float startSpacing)
    {
        _cardsArea = GetComponent<CardsArea>();
        _cardsArea.Initialize(sizeArea, startSpacing);

        _waitShowEndRound = new(_timeShowEndRound);
        _waitShowEndLevel = new(_timeShowEndLevel);
    }

    public Coroutine StartLevel_Routine(LevelSetupData data)
    {
        _data = data;
        _delayTurn = _timeTurnPerAll / data.CountShapes;
        int size = data.Size;

        _isFind = false;
        EventStartLevel?.Invoke();

        _cardsArea.CreateCards(size, OnCardSelected);
        SetupCards(true);
        return _cardsArea.Turn90Random(_delayTurn);
    }

    public void Run()
    {
        _cardsArea.ForEach((c) => c.IsInteractable = true);
        EventStartRound?.Invoke();
    }

    public void Stop()
    {
        _cardsArea.ForEach((c) => c.CheckCroup(0));
        StartCoroutine(EndLevel_Coroutine(!_isFind));
    }

    private void SetupCards(bool isNew)
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
                    card.Setup(shape, axis, i, _isCheats);
                else
                    card.ReSetup(shape, axis, i, _isCheats);
            }
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
            _creatorShapes.Create(_data.Count, _data.CountShuffle == 0);
            Stack<Shape> shs = new(_data.Count);
            Shape sh;
            Color color = Color.white; color.Randomize(_saturationMin, _brightnessMin);

            for (int i = 0; i < _data.Count; i++)
            {
                sh = new(_creatorShapes.Next, color);
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
            StartCoroutine(EndLevel_Coroutine(true));

        #region Local function
        IEnumerator NextRound_Coroutine()
        {
            _isFind = true;
            yield return _waitShowEndRound;
            SetupCards(false);
            yield return _cardsArea.TurnRandom(_delayTurn);
            Run();
        }
        #endregion
    }

    private IEnumerator EndLevel_Coroutine(bool isGameOver)
    {
        yield return _waitShowEndLevel;
        yield return _cardsArea.Turn90Random(_delayTurn / 2f);

        EventEndLevel?.Invoke(isGameOver);
    }

    #region Nested Classe
    //***********************************
    [Serializable]
    private class CreatorShapes
    {
        [SerializeField] private Sprite[] _mainSprites;
        [SerializeField] private Sprite[] _centerSprites;
        [SerializeField] private Sprite[] _outerSprites;

        public Sprite[] Next => _sprites.Dequeue();

        private readonly Queue<Sprite[]> _sprites = new();

        private const int COUNT = 6;
        private const int COUNT_SPRITES = 9;

        public void Create(int count, bool isSimilar)
        {
            _sprites.Clear();
            Sprite temp = null;
            for (int i = 0; i < count; i++)
            {
                Sprite[] sprites = new Sprite[COUNT];
                do
                {
                    sprites[0] = _mainSprites[Random.Range(0, COUNT_SPRITES)];
                    sprites[1] = _centerSprites[Random.Range(0, COUNT_SPRITES)];
                    for (int j = 2; j < COUNT; j++)
                    {
                        if (!isSimilar || j == 2)
                            temp = _outerSprites[Random.Range(0, COUNT_SPRITES)];
                        sprites[j] = temp;
                    }
                }
                while (_sprites.Contains(sprites, Comparison));

                _sprites.Enqueue(sprites);
            }

            #region Local functions
            //===============================
            static bool Comparison(Sprite[] spritesA, Sprite[] spritesB)
            {
                for (int i = 0; i < COUNT; i++)
                {
                    if (spritesA[i] != spritesB[i])
                        return false;
                }
                return true;
            }
            #endregion
        }
        #endregion
    }
}
