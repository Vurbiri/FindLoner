using System.Collections;
using UnityEngine;

public class HelpController : MonoBehaviour
{
    [SerializeField] private AHelpControlElement _controlDesktop;
    [SerializeField] private AHelpControlElement _controlMobile;
    [Space]
    [SerializeField] private Vector2 _startPosition = new(3.9f, -4.55f);
    [SerializeField] private float _distanceUpDown = 3.1f;
    [Space]
    [SerializeField] private float _timeMoveUp = 1.5f;
    [SerializeField] private float _timeSwitchUp = 0.5f;

    private AHelpControlElement _controlCurrent;
    private RectTransform _thisRectTransform;

    private WaitForSecondsRealtime _waitSwitchUp;
    private Vector2 _endPositionUp;
    private Vector2 _speedUp;
    private Vector2 _endPositionDown;
    private Vector2 _speedDown;

    private Vector2 _endPositionLeft;
    private WaitForSecondsRealtime _waitSwitchLeft;
    private Vector2 _speedLeft;


    public void Initialize(Vector2 speedLeft, float distance, float timeSwitch, bool IsDesktop)
    {
        _controlCurrent = IsDesktop ? _controlDesktop : _controlMobile;
        _thisRectTransform = GetComponent<RectTransform>();
        
        _endPositionUp = _startPosition; 
        _endPositionUp.y += _distanceUpDown;
        _speedUp = (_endPositionUp - _startPosition) / _timeMoveUp;
        

        _endPositionLeft = _endPositionUp;
        _endPositionLeft.x -= distance;
        _speedLeft = speedLeft;

        _endPositionDown = _endPositionLeft;
        _endPositionDown.y -= _distanceUpDown;
        _speedDown = -_speedUp;

        _waitSwitchUp = new(_timeSwitchUp);
        _waitSwitchLeft = new(timeSwitch);
    }

    public IEnumerator MoveUpCoroutine()
    {
        _controlCurrent.StateOff();

        do
        {
            _thisRectTransform.anchoredPosition += _speedUp * Time.unscaledDeltaTime;
            yield return null;
        }
        while (_thisRectTransform.anchoredPosition.y < _endPositionUp.y);
        _thisRectTransform.anchoredPosition = _endPositionUp;

        yield return _waitSwitchUp;
        _controlCurrent.StateOn();
        yield return _waitSwitchUp;
    }

    public IEnumerator MoveLeftCoroutine()
    {
        do
        {
            _thisRectTransform.anchoredPosition += _speedLeft * Time.unscaledDeltaTime;
            yield return null;
        }
        while (_thisRectTransform.anchoredPosition.x > _endPositionLeft.x);

        _thisRectTransform.anchoredPosition = _endPositionLeft;

        yield return _waitSwitchLeft;
        _controlCurrent.StateOff();
    }

    public IEnumerator MoveDownCoroutine()
    {
        yield return _waitSwitchUp;

        do
        {
            _thisRectTransform.anchoredPosition += _speedDown * Time.unscaledDeltaTime;
            yield return null;
        }
        while (_thisRectTransform.anchoredPosition.y > _endPositionDown.y);
        _thisRectTransform.anchoredPosition = _endPositionDown;
    }

    public void ResetState()
    {
        _thisRectTransform.anchoredPosition = _startPosition;
        _controlCurrent.StateOff();
    }
}
