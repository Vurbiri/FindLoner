using System;
using System.Collections;
using UnityEngine;

public class TimeCardsArea : ACardsArea<TimeCard>
{
    private Func<float, IEnumerator>[] _turnsToValue;

    protected override void Awake()
    {
        base.Awake();

        _turnsToValue = new Func<float, IEnumerator>[] { TurnToValue_FX_FY_Coroutine, TurnToValue_FX_BY_Coroutine, TurnToValue_BX_FY_Coroutine, TurnToValue_BX_BY_Coroutine,
                                                         TurnToValue_FY_FX_Coroutine, TurnToValue_FY_BX_Coroutine, TurnToValue_BY_FX_Coroutine, TurnToValue_BY_BX_Coroutine};

    }

    public Coroutine TurnToValueRandom(float delay) => StartCoroutine(_turnsToValue[_index = UnityEngine.Random.Range(0, COUNT_FUNC)](delay));

    public Coroutine TurnToValueRepeat(float delay) => StartCoroutine(_turnsToValue[_index](delay));


    #region TurnToValue
    //1
    private IEnumerator TurnToValue_FX_FY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].TurnToValue_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //2
    private IEnumerator TurnToValue_FX_BY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = 0; x < _size; x++)
        {
            for (int y = _size - 1; y >= 0; y--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].TurnToValue_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //3
    private IEnumerator TurnToValue_BX_FY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = _size - 1; x >= 0; x--)
        {
            for (int y = 0; y < _size; y++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].TurnToValue_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //4
    private IEnumerator TurnToValue_BX_BY_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int x = _size - 1; x >= 0; x--)
        {
            for (int y = _size - 1; y >= 0; y--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].TurnToValue_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //5
    private IEnumerator TurnToValue_FY_FX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = 0; y < _size; y++)
        {
            for (int x = 0; x < _size; x++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].TurnToValue_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //6
    private IEnumerator TurnToValue_FY_BX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = 0; y < _size; y++)
        {
            for (int x = _size - 1; x >= 0; x--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].TurnToValue_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //7
    private IEnumerator TurnToValue_BY_FX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = _size - 1; y >= 0; y--)
        {
            for (int x = 0; x < _size; x++)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].TurnToValue_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    //8
    private IEnumerator TurnToValue_BY_BX_Coroutine(float time)
    {
        Coroutine coroutine = null;
        for (int y = _size - 1; y >= 0; y--)
        {
            for (int x = _size - 1; x >= 0; x--)
            {
                coroutine = StartCoroutine(_cardsArea[x, y].TurnToValue_Coroutine());
                yield return new WaitForSeconds(time);
            }
        }
        yield return coroutine;
    }
    #endregion
}
