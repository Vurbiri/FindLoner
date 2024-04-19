using System.Collections;
using TMPro;
using UnityEngine;

public abstract class ABoard : MonoBehaviour
{
    [SerializeField] protected TMP_Text _textBoard;
    [Space]
    [SerializeField] private float _timeChangeValue = 2f;

    protected float _current, _maxValue = 0.01f;

    protected void SetMaxValue(int value) => SetMaxValue((float)value);
    protected void SetMaxValue(float value)
    {
        StopAllCoroutines();

        _maxValue = value <= 0 ? 0.01f : value;
        _current = 0;
        SetSmoothValue(value);
    }

    protected void SetValue(int value) => SetValue((float)value);
    protected void SetValue(float value)
    {
        StopAllCoroutines();

        _current = value;
        ToText(Mathf.RoundToInt(value));
    }

    protected void SetSmoothValue(int value) => SetSmoothValue((float)value);
    protected void SetSmoothValue(float value)
    {
        StopAllCoroutines();

        StartCoroutine(SetValueSmooth_Coroutine());

        #region Local functions
        //=======================
        IEnumerator SetValueSmooth_Coroutine()
        {
            yield return StartCoroutine(SmoothChangeValue_Coroutine(value, _timeChangeValue));
            _current = value;
            ToText(Mathf.RoundToInt(value));
        }
        #endregion
    }

    protected void SetSmoothValueForMaxValue(int value) => SetSmoothValueForMaxValue((float)value);
    protected void SetSmoothValueForMaxValue(float value)
    {
        StopAllCoroutines();
        
        StartCoroutine(SetValueSmoothForMaxValue_Coroutine());

        #region Local functions
        //=======================
        IEnumerator SetValueSmoothForMaxValue_Coroutine()
        {
            yield return StartCoroutine(SmoothChangeValue_Coroutine(value, Mathf.Abs((_current - value) / _maxValue) * _timeChangeValue));
            _current = value;
            ToText(Mathf.RoundToInt(value));
        }
        #endregion
    }

    protected void Clear()
    {
        StopAllCoroutines();

        _current = 0;
        TextDefault();
    }

    protected void ClearSmoothForMaxValue()
    {
        StopAllCoroutines();

        StartCoroutine(ClearSmoothForMaxValue_Coroutine());

        #region Local functions
        //=======================
        IEnumerator ClearSmoothForMaxValue_Coroutine()
        {
            yield return StartCoroutine(SmoothChangeValue_Coroutine(0f, (_current / _maxValue) * _timeChangeValue));

            _current = 0;
            TextDefault();
        }
        #endregion
    }

    protected abstract void TextDefault();
    protected abstract void ToText(int value);

    private IEnumerator SmoothChangeValue_Coroutine(float value, float time)
    {
        float speed = (value - _current) / time;
        
        if (Mathf.Abs(speed) < 0.1f)
            yield break;

        int current, old = Mathf.RoundToInt(_current);

        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
            current = Mathf.RoundToInt(_current += speed * Time.deltaTime);

            if (current != old)
                ToText(old = current);
        }
    }

}
