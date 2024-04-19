using UnityEngine;

public class TimeBoard : ABoard
{
    [Space]
    [SerializeField] private BonusLevels _bonusLevels;
    [SerializeField] private Timer _timer;

    private void Awake()
    {
        Clear();

        _timer.EventSetTime += SetMaxValue;
        _timer.EventTick += SetValue;
        _timer.EventStop += ClearSmoothForMaxValue;

        _bonusLevels.EventSetTime += SetSmoothValue;
        _bonusLevels.EventAddTime += SetSmoothValue;
        //_bonusLevels.EventEndLevel += () => _time.text = "0";
    }

    protected override void TextDefault() => _textBoard.text = "0:00";
    protected override void ToText(int value) => _textBoard.text = $"{(value / 60)}:{value % 60:D2}";
}
