using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _timeStep = 1f;

    private float _time;
    private bool _pause;
    private WaitForSeconds _waitTime;
    private Coroutine _coroutine;

    public float MaxTime { get => _time; set { _time = value; EventSetTime?.Invoke(value); } }
    public bool IsPause { get => _pause; set => _pause = value; }

    public event Action<float> EventSetTime;
    public event Action<float> EventTick;
    public event Action EventStop;
    public event Action EventEndTime;

    private void Awake()
    {
        _waitTime = new(_timeStep);
    }

    public void Run()
    {
        _pause = false;
        _coroutine = StartCoroutine(Run_Coroutine());

        #region Local function
        //=========================================================
        IEnumerator Run_Coroutine()
        {
            while (_time > 0)
            {
                yield return null;
                if (_pause) continue;

                _time -= Time.deltaTime;
                EventTick?.Invoke(_time);
            }

            _time = 0;
            _coroutine = null;
            EventEndTime?.Invoke();
        }
        //IEnumerator Run_Coroutine()
        //{
        //    while (_time > 0)
        //    {
        //        yield return _waitTime;
        //        if (_pause) continue;

        //        _time -= _timeStep;//Time.deltaTime;
        //        EventTick?.Invoke(_time);
        //    }

        //    _time = 0;
        //    _coroutine = null;
        //    EventEndTime?.Invoke();
        //}
        #endregion
    }

    public void Stop()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);

            _time = 0;
            _coroutine = null;
            EventStop?.Invoke();
        }
    }

    
}
