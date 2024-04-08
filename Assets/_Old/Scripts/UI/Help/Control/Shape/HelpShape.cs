using System.Collections;
using UnityEngine;

public class HelpShape : MonoBehaviour
{
    [SerializeField] private HelpBlock[] _blocks;
    [Space]
    [SerializeField] private Vector2 _startPosition = new(-1.4f, 0f);
    [SerializeField] private Vector2 _projectionPosition = new(-7.2f, -0.25f);
    [Space]
    [SerializeField] private float _projectionAlfa = 0.35f;
    [Space]
    [SerializeField] private Vector2 _stepMove = Vector2.left;

    private RectTransform _thisRectTransform;

    private Vector2 _endPosition;
    private WaitForSecondsRealtime _waitSwitch;
    private Vector2 _speed;
    private int _stepCount;

    private WaitForSecondsRealtime _waitStep;

    public void Initialize(Vector2 speed, float distance, float stepCount, float timeMoveStepTwo, float timeSwitch)
    {
        _thisRectTransform = GetComponent<RectTransform>();
        foreach (var block in _blocks)
            block.Initialize();

        _endPosition = _startPosition;
        _endPosition.x -= distance;

        _speed = speed;
        _stepCount = Mathf.RoundToInt(stepCount);

        _waitSwitch = new(timeSwitch);
        _waitStep = new(timeMoveStepTwo / stepCount);
    }

    public IEnumerator MoveLeftCoroutine()
    {
        do
        {
            _thisRectTransform.anchoredPosition += _speed * Time.unscaledDeltaTime;
            yield return null;
        }
        while (_thisRectTransform.anchoredPosition.x > _endPosition.x);

        _thisRectTransform.anchoredPosition = _projectionPosition;
        ProjectionOn();

        for(int i = 0; i < _stepCount; i++) 
        {
            yield return _waitStep;
            _thisRectTransform.anchoredPosition += _stepMove;
        }

        yield return _waitSwitch;
        ProjectionOff();
    }


    public void ResetState()
    {
        _thisRectTransform.anchoredPosition = _startPosition;
        ProjectionOff();
    }

    private void ProjectionOn()
    {
        foreach (var block in _blocks)
            block.SetColorAlfa(_projectionAlfa);
    }

    private void ProjectionOff()
    {
        foreach (var block in _blocks)
            block.SetColorAlfa(1f);
    }
}
