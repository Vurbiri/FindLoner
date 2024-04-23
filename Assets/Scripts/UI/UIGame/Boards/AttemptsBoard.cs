using UnityEngine;

public class AttemptsBoard : ABoard
{
    [Space]
    [SerializeField] private BonusLevels _bonusLevels;

    private void Start()
    {
        Clear();

        _bonusLevels.EventSetMaxAttempts += SetMaxValue;
        _bonusLevels.EventChangedAttempts += SetValue;
        _bonusLevels.EventEndLevel += _ => ClearSmoothForMaxValue();
    }

    protected override void TextDefault() => _textBoard.text = "0";
    protected override void ToText(int value) => _textBoard.text = value.ToString();


}
