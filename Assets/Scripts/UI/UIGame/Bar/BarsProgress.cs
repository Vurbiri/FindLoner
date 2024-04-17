using System.Collections;
using UnityEngine;

public class BarsProgress : MonoBehaviour
{
    [SerializeField] private BonusLevels _bonusLevels;
    [Space]
    [SerializeField] private BarProgress _barRight;
    [SerializeField] private BarProgress _barLeft;
    [Space]
    [SerializeField] private Color _colorNormal = Color.green;
    [SerializeField] private Color _colorWarning = Color.yellow;
    [SerializeField] private Color _colorDanger = Color.red;
    [Space]
    [SerializeField] private float _valueWarning = 0.7f;
    [SerializeField] private float _valueDanger = 0.35f;
    [Space]
    [SerializeField] private float _speedChangeValue = 1.5f;

    private float _maxValue;
    private Coroutine _coroutine;

    private void Awake()
    {
        Clear();

        _bonusLevels.EventChangedMaxAttempts += (v) => SetMaxValue(v);
        _bonusLevels.EventChangedAttempts += SetAttempts;
        _bonusLevels.EventEndLevel += Clear;

        #region Local functions
        //===========================================================================================
        void Clear() => _barRight.Value = _barLeft.Value = 0;
        void SetMaxValue(float value) => _barRight.MaxValue = _barLeft.MaxValue = _maxValue = value;
        void SetValue(float value)
        {
            _barRight.Value = _barLeft.Value = value;
            SetColor();

            #region Local function
            //============================================
            void SetColor()
            {
                value /= _maxValue;

                if(value >= _valueWarning)
                    _barRight.Color = _barLeft.Color = _colorNormal;
                else if(value >= _valueDanger)
                    _barRight.Color = _barLeft.Color = _colorWarning;
                else
                    _barRight.Color = _barLeft.Color = _colorDanger;
            }
            #endregion
        }
        void SetAttempts(int attempts)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(SetValue_Coroutine());

            #region Local function
            //============================================
            IEnumerator SetValue_Coroutine()
            {
                float current = _barRight.Value;
                while (current > attempts)
                {
                    yield return null;
                    SetValue(current -= _speedChangeValue * Time.deltaTime);
                }
                SetValue(attempts);
                _coroutine = null;
            }
            #endregion
        }
        #endregion
    }
}
