
public class ScoreBoard : ABoard
{
    private void Start()
    {
        SetText(_dataGame.Score.ToString());
        _dataGame.EventChangeScore += SetValue;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeScore -= SetValue;
    }

    private void SetValue(long value) => SetText(value.ToString());
}
