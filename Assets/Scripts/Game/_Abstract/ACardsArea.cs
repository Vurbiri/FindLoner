using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACardsArea<T, U> : MonoBehaviour, IEnumerable<T> where T : ACard
{
    [SerializeField] protected T _prefabCard;
    [SerializeField] protected Transform _repository;
    protected Transform _thisTransform;

    protected readonly FreeStack<T> _cardsActive = new(CAPACITY_LIST);
    protected readonly ShuffledArray<T> _cardsRandom = new(CAPACITY_LIST);
    protected readonly Stack<T> _cardsRepository = new(CAPACITY_STACK);

    protected int _countCellSide, _countCell;
    private float _sizeArea = 920, _startSpacing = 12f;

    private Func<float, Func<T, IEnumerator>, IEnumerator>[] _funcTraversing;
    private int _indexFunc;

    private const int COUNT_FUNC = 8;
    private const int CAPACITY_LIST = 144;
    private const int CAPACITY_STACK = 140;

    public T RandomCard => _cardsRandom.Next;
    public bool TryGetRandomCard(out T card) => _cardsRandom.TryGetNext(out card);

    public void Initialize(float sizeArea, float startSpacing)
    {
        _sizeArea = sizeArea; 
        _startSpacing = startSpacing;
        
        _thisTransform = transform;

        _funcTraversing = new Func<float, Func<T, IEnumerator>, IEnumerator>[]
            { Traversing_FX_FY_Coroutine, Traversing_FX_BY_Coroutine, Traversing_BX_FY_Coroutine, Traversing_BX_BY_Coroutine,
              Traversing_FY_FX_Coroutine, Traversing_FY_BX_Coroutine, Traversing_BY_FX_Coroutine, Traversing_BY_BX_Coroutine};
    }

    public void Shuffle() => _cardsRandom.Shuffle();
        
    public Coroutine Turn90Random(float delay) => StartCoroutine(_funcTraversing[_indexFunc = UnityEngine.Random.Range(0, COUNT_FUNC)](delay, Turn90_Coroutine));
    public Coroutine Turn90Repeat(float delay) => StartCoroutine(_funcTraversing[_indexFunc](delay, Turn90_Coroutine));
    private IEnumerator Turn90_Coroutine(T card) => card.Turn90_Coroutine();
    
    public void CreateCards(int size, U obj)
    {
        int countNew = size * size;
        if (_cardsActive.Count == countNew)
            return;

        T card;
        while (_cardsActive.Count > countNew)
        {
            card = _cardsActive.Pop();
            card.Deactivate(_repository);
            _cardsRepository.Push(card);

        }
        while (_cardsActive.Count < countNew)
        {
            if (_cardsRepository.Count > 0)
            {
                card = _cardsRepository.Pop();
                card.Activate(_thisTransform);
            }
            else
            {
                card = Instantiate(_prefabCard, _thisTransform);
                AdditionalActionsCreatingCard(card, obj);
            }

            _cardsActive.Push(card);
        }

        _cardsActive.Size = _countCellSide = size;
        _cardsRandom.ReSize(size);

        float cellSize = _sizeArea / size, startPos = cellSize / 2f;
        Vector2 cardSize = Vector2.one * (cellSize - _startSpacing / size);
        
        for (int x = 0; x < _countCellSide; x++)
        {
            for (int y = 0; y < _countCellSide; y++)
            {
                card = _cardsActive[x, y];
                card.LocalPosition = new(startPos + x * cellSize, startPos + y * cellSize, 0f);
                card.SetSize(cardSize);
                _cardsRandom[x, y] = card;
            }
        }
    }

    public void ForEach(Action<T> action)
    {
        foreach (var item in _cardsActive)
            action(item);
    }

    protected virtual void AdditionalActionsCreatingCard(T card, U obj)
    {

    }

    public IEnumerator<T> GetEnumerator() => _cardsActive.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _cardsActive.GetEnumerator();

    #region Traversing
    protected Coroutine TraversingRandom(float delay, Func<T, IEnumerator> funcCoroutine) => StartCoroutine(_funcTraversing[_indexFunc = UnityEngine.Random.Range(0, COUNT_FUNC)](delay, funcCoroutine));
    protected Coroutine TraversingRepeat(float delay, Func<T, IEnumerator> funcCoroutine) => StartCoroutine(_funcTraversing[_indexFunc](delay, funcCoroutine));

    //1
    private IEnumerator Traversing_FX_FY_Coroutine(float time, Func<T, IEnumerator> funcCoroutine)
    {
        Coroutine coroutine = null;
        for (int x = 0; x < _countCellSide; x++)
        {
            for (int y = 0; y < _countCellSide; y++)
            {
                coroutine = StartCoroutine(funcCoroutine(_cardsActive[x, y]));
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //2
    private IEnumerator Traversing_FX_BY_Coroutine(float time, Func<T, IEnumerator> funcCoroutine)
    {
        Coroutine coroutine = null;
        for (int x = 0; x < _countCellSide; x++)
        {
            for (int y = _countCellSide - 1; y >= 0; y--)
            {
                coroutine = StartCoroutine(funcCoroutine(_cardsActive[x, y]));
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //3
    private IEnumerator Traversing_BX_FY_Coroutine(float time, Func<T, IEnumerator> funcCoroutine)
    {
        Coroutine coroutine = null;
        for (int x = _countCellSide - 1; x >= 0; x--)
        {
            for (int y = 0; y < _countCellSide; y++)
            {
                coroutine = StartCoroutine(funcCoroutine(_cardsActive[x, y]));
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //4
    private IEnumerator Traversing_BX_BY_Coroutine(float time, Func<T, IEnumerator> funcCoroutine)
    {
        Coroutine coroutine = null;
        for (int x = _countCellSide - 1; x >= 0; x--)
        {
            for (int y = _countCellSide - 1; y >= 0; y--)
            {
                coroutine = StartCoroutine(funcCoroutine(_cardsActive[x, y]));
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //5
    private IEnumerator Traversing_FY_FX_Coroutine(float time, Func<T, IEnumerator> funcCoroutine)
    {
        Coroutine coroutine = null;
        for (int y = 0; y < _countCellSide; y++)
        {
            for (int x = 0; x < _countCellSide; x++)
            {
                coroutine = StartCoroutine(funcCoroutine(_cardsActive[x, y]));
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //6
    private IEnumerator Traversing_FY_BX_Coroutine(float time, Func<T, IEnumerator> funcCoroutine)
    {
        Coroutine coroutine = null;
        for (int y = 0; y < _countCellSide; y++)
        {
            for (int x = _countCellSide - 1; x >= 0; x--)
            {
                coroutine = StartCoroutine(funcCoroutine(_cardsActive[x, y]));
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //7
    private IEnumerator Traversing_BY_FX_Coroutine(float time, Func<T, IEnumerator> funcCoroutine)
    {
        Coroutine coroutine = null;
        for (int y = _countCellSide - 1; y >= 0; y--)
        {
            for (int x = 0; x < _countCellSide; x++)
            {
                coroutine = StartCoroutine(funcCoroutine(_cardsActive[x, y]));
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //8
    private IEnumerator Traversing_BY_BX_Coroutine(float time, Func<T, IEnumerator> funcCoroutine)
    {
        Coroutine coroutine = null;
        for (int y = _countCellSide - 1; y >= 0; y--)
        {
            for (int x = _countCellSide - 1; x >= 0; x--)
            {
                coroutine = StartCoroutine(funcCoroutine(_cardsActive[x, y]));
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    #endregion
}
