using UnityEngine;

public class TimeBoard : ABoard
{
    [Space]
    [SerializeField] private BonusLevels _bonusLevels;
    [SerializeField] private Timer _timerGameLevel;

    private void Start()
    {
        Clear();

        _timerGameLevel.EventSetTime += SetMaxValue;
        _timerGameLevel.EventTick += SetValue;
        _timerGameLevel.EventEndTime += Clear;
        _timerGameLevel.EventStop += ClearSmoothForMaxValue;

        _bonusLevels.EventSetTime += SetMaxValue;
        _bonusLevels.EventAddTime += SetSmoothValueAndMaxValue;
    }

    protected override void TextDefault() => _textBoard.text = "0:00";
    protected override void ToText(int value) => _textBoard.text = value.ToStringTime();
}
