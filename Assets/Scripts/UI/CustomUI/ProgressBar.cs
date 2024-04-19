using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform _fill;
    [Space]
    [SerializeField, Dropdown("GetAxisValues")] private int _axe = 1;
    [SerializeField] private float _speedChangeValue = 0.6f;
    [Space]
    [SerializeField] private Color _colorNormal = Color.green;
    [SerializeField] private Color _colorWarning = Color.yellow;
    [SerializeField] private Color _colorDanger = Color.red;
    [Space]
    [SerializeField] private float _valueWarning = 0.7f;
    [SerializeField] private float _valueDanger = 0.35f;

    private float _maxValue = 10f, _progress = 0f;
    private Graphic _imageFill;
    private Vector2 _anchorMin = Vector2.zero, _anchorMax = Vector2.one;
    private Coroutine _coroutine;

    public float MaxValue
    {
        set
        {
            _maxValue = value <= 0 ? 0.01f : value;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _progress = 0f;
            _coroutine = StartCoroutine(SetProgress_Coroutine(1f));
        }
    }

    public float Value 
    { 
        set 
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            SetProgress(Mathf.Clamp01(value / _maxValue));
        } 
    }

    public float SmoothValue
    {
        set
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(SetProgress_Coroutine(Mathf.Clamp01(value / _maxValue)));
        }
    }

    private void Awake()
    {
        _imageFill = _fill.GetComponent<Graphic>();

        SetProgress(0f);
    }

    private void SetProgress(float progressNew, bool staticColor = false)
    {
        _progress = progressNew;

        //Canvas.ForceUpdateCanvases();

        // Set anchor
        _anchorMin[_axe] = 0.5f - progressNew * 0.5f;
        _anchorMax[_axe] = 0.5f + progressNew * 0.5f;

        _fill.anchorMin = _anchorMin;
        _fill.anchorMax = _anchorMax;

        //Set color
        if (staticColor)
        {
            _imageFill.color = _colorNormal;
            return;
        }
        
        if (progressNew >= _valueWarning)
            _imageFill.color = _colorNormal;
        else if (progressNew >= _valueDanger)
            _imageFill.color = _colorWarning;
        else
            _imageFill.color = _colorDanger;
    }

    private IEnumerator SetProgress_Coroutine(float progressNew)
    {
        bool isUp = (progressNew - _progress) >= 0f;
        float sign = isUp ? 1f : -1f;
        while (_progress * sign < progressNew * sign)
        {
            yield return null;
            SetProgress(_progress + _speedChangeValue * sign * Time.deltaTime, isUp);
        }
        SetProgress(progressNew);
        _coroutine = null;
    }

#if UNITY_EDITOR
    private DropdownList<int> GetAxisValues()
    {
        return new DropdownList<int>()
        {
            { "X",  0 },
            { "Y",  1 }
        };
    }
#endif
}
