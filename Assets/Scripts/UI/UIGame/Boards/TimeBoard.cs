using UnityEngine;

public class TimeBoard : ABoard
{
    [Space]
    [SerializeField] private BonusLevels _bonusLevels;
    [SerializeField] private Timer _timer;

    private void Start()
    {
        Clear();

        _timer.EventSetTime += SetMaxValue;
        _timer.EventTick += SetValue;
        _timer.EventEndTime += Clear;
        _timer.EventStop += ClearSmoothForMaxValue;

        _bonusLevels.EventSetTime += SetMaxValue;
        _bonusLevels.EventAddTime += SetSmoothValueAndMaxValue;
    }

    protected override void TextDefault() => _textBoard.text = "0:00";
    protected override void ToText(int value) => _textBoard.text = $"{(value / 60)}:{value % 60:D2}";
}
