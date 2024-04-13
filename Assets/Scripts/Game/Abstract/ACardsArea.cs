using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACardsArea<T> : MonoBehaviour where T : ACard
{
    [SerializeField] private T _prefabCard;
    [SerializeField] private Transform _repository;
    private Transform _thisTransform;

    private readonly Stack<T> _cardsActive = new();
    private readonly Stack<T> _cardsRepository = new();
    protected T[,] _cardsArea;
    protected int _size, _index;

    private Func<float, IEnumerator>[] _shows;
    private Func<float, IEnumerator>[] _turnsOne;
    protected const int COUNT_FUNC = 8;

    public List<T> Cards => new(_cardsActive);

    protected virtual void Awake()
    {
        _thisTransform = transform;

        _shows = new Func<float, IEnumerator>[] { Show_FX_FY_Coroutine, Show_FX_BY_Coroutine, Show_BX_FY_Coroutine, Show_BX_BY_Coroutine,
                                                  Show_FY_FX_Coroutine, Show_FY_BX_Coroutine, Show_BY_FX_Coroutine, Show_BY_BX_Coroutine};

        _turnsOne = new Func<float, IEnumerator>[] { Turn_FX_FY_Coroutine, Turn_FX_BY_Coroutine, Turn_BX_FY_Coroutine, Turn_BX_BY_Coroutine,
                                                     Turn_FY_FX_Coroutine, Turn_FY_BX_Coroutine, Turn_BY_FX_Coroutine, Turn_BY_BX_Coroutine};
    }
       

    public Coroutine ShowRandom(float delay) => StartCoroutine(_shows[_index = UnityEngine.Random.Range(0, COUNT_FUNC)](delay));
    public Coroutine TurnRandom(float delay) => StartCoroutine(_turnsOne[_index = UnityEngine.Random.Range(0, COUNT_FUNC)](delay));

    public Coroutine ShowRepeat(float delay) => StartCoroutine(_shows[_index](delay));
    public Coroutine TurnRepeat(float delay) => StartCoroutine(_turnsOne[_index](delay));

    public void ForEach(Action<T> action)
    {
        foreach (var item in _cardsActive)
            action(item);
    }

    public void CreateCards(int sizeNew, Action<ACard> action)
    {
        int countNew = sizeNew * sizeNew;
        if (_cardsActive.Count == countNew) return;

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
                card.EventSelected += action;
            }

            _cardsActive.Push(card);
        }

        _cardsArea = new T[sizeNew, sizeNew];
        int x = 0, y = 0;
        foreach (var c in _cardsActive)
        {
            _cardsArea[x, y++] = c;

            if (y == sizeNew)
            {
                x++; y = 0;
            }
        }

        _size = sizeNew;
    }

    #region Show
    //1
    private IEnumerator Show_FX_FY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Show_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //2
    private IEnumerator Show_FX_BY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = 0; x < _size; x++)
        {
            for (int y = _size - 1; y >= 0; y--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Show_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //3
    private IEnumerator Show_BX_FY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = _size - 1; x >= 0; x--)
        {
            for (int y = 0; y < _size; y++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Show_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //4
    private IEnumerator Show_BX_BY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = _size - 1; x >= 0; x--)
        {
            for (int y = _size - 1; y >= 0; y--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Show_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //5
    private IEnumerator Show_FY_FX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = 0; y < _size; y++)
        {
            for (int x = 0; x < _size; x++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Show_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //6
    private IEnumerator Show_FY_BX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = 0; y < _size; y++)
        {
            for (int x = _size - 1; x >= 0; x--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Show_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //7
    private IEnumerator Show_BY_FX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = _size - 1; y >= 0; y--)
        {
            for (int x = 0; x < _size; x++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Show_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //8
    private IEnumerator Show_BY_BX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = _size - 1; y >= 0; y--)
        {
            for (int x = _size - 1; x >= 0; x--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Show_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    #endregion

    #region Turn
    //1
    private IEnumerator Turn_FX_FY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Turn_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //2
    private IEnumerator Turn_FX_BY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = 0; x < _size; x++)
        {
            for (int y = _size - 1; y >= 0; y--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Turn_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //3
    private IEnumerator Turn_BX_FY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = _size - 1; x >= 0; x--)
        {
            for (int y = 0; y < _size; y++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Turn_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //4
    private IEnumerator Turn_BX_BY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = _size - 1; x >= 0; x--)
        {
            for (int y = _size - 1; y >= 0; y--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Turn_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //5
    private IEnumerator Turn_FY_FX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = 0; y < _size; y++)
        {
            for (int x = 0; x < _size; x++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Turn_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //6
    private IEnumerator Turn_FY_BX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = 0; y < _size; y++)
        {
            for (int x = _size - 1; x >= 0; x--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Turn_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //7
    private IEnumerator Turn_BY_FX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = _size - 1; y >= 0; y--)
        {
            for (int x = 0; x < _size; x++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Turn_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //8
    private IEnumerator Turn_BY_BX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = _size - 1; y >= 0; y--)
        {
            for (int x = _size - 1; x >= 0; x--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].Turn_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    #endregion
}
