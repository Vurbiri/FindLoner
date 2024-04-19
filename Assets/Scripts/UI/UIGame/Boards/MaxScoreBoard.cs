
public class MaxScoreBoard : ABoardScore
{
    private void Start()
    {
        SetSmoothValue(_dataGame.MaxScore);
        _dataGame.EventChangeMaxScore += SetSmoothValue;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeMaxScore -= SetSmoothValue;
    }
}
