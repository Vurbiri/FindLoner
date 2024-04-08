using System.Collections;
using UnityEngine;

public class HelpExampleControl : MonoBehaviour
{
    [SerializeField] private HelpController _controller;
    [SerializeField] private HelpShape _shape;
    [Space]
    [SerializeField] private float _distanceStepOne = 5.5f;
    [SerializeField] private float _distanceStepTwo = 2f;
    [Space]
    [SerializeField] private float _timeMove = 5f;
    [SerializeField] private float _timeSwitch = 1f;
    [Space]
    [SerializeField] private float _timeStart = 0.75f;
    [SerializeField] private float _timeEnd = 1.75f;
    [Space]
    [SerializeField] private TextLocalization _text;
    [SerializeField] private string _keyDesktop = "GameControlDesktop";
    [SerializeField] private string _keyMobile = "GameControlMobile";

    private Coroutine _coroutine;

    private WaitForSecondsRealtime _waitStart;
    private WaitForSecondsRealtime _waitEnd;

    private void Awake()
    {
        bool isDesktop = SettingsGame.Instance.IsDesktop;
        _text.Setup(isDesktop ? _keyDesktop : _keyMobile);

        Vector2 speedLeft = Vector2.zero;
        speedLeft.x = -(_distanceStepOne + _distanceStepTwo) / _timeMove;
        float timeMoveStepTwo = _distanceStepTwo / -speedLeft.x;

        _controller.Initialize(speedLeft, _distanceStepOne + _distanceStepTwo, _timeSwitch, isDesktop);
        _shape.Initialize(speedLeft, _distanceStepOne, _distanceStepTwo, timeMoveStepTwo, _timeSwitch);

        _waitStart = new(_timeStart);
        _waitEnd = new(_timeEnd);
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(ExampleDemo());
    }


    private IEnumerator ExampleDemo()
    {
        while (true)
        {
            _controller.ResetState();
            _shape.ResetState();

            yield return _waitStart;

            yield return StartCoroutine(_controller.MoveUpCoroutine());

            StartCoroutine(_shape.MoveLeftCoroutine());
            yield return StartCoroutine(_controller.MoveLeftCoroutine());

            yield return StartCoroutine(_controller.MoveDownCoroutine());

            yield return _waitEnd;
        }
    }

    private void OnDisable()
    {
        if (_coroutine == null)
            return;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }
}
