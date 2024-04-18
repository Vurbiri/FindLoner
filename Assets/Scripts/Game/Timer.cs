using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField, Tooltip("ms")] private int _timeStep = 100;

    private int _time;
    private bool _pause;
    private WaitForSeconds _waitTime;
    private Coroutine _coroutine;

    public int Time { get => _time; set { _time = value; EventSetTime?.Invoke(value); } }
    public bool IsPause { get => _pause; set => _pause = value; }

    public event Action<int> EventSetTime;
    public event Action<int> EventTick;

    private void Awake()
    {
        _waitTime = new(_timeStep / 1000f);
    }

    public void Run()
    {
        _coroutine = StartCoroutine(Run_Coroutine());

        #region Local function
        //=========================================================
        IEnumerator Run_Coroutine()
        {
            while (_time > 0)
            {
                yield return _waitTime;
                if (_pause) continue;

                _time -= _timeStep;
                EventTick?.Invoke(_time);
            }

            _time = 0;
            _coroutine = null;
        }
        #endregion
    }

    public void Stop()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
            _time = 0;
        }
    }

    
}
