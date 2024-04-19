
public class ScoreBoard : ABoardScore
{
    private void Start()
    {
        SetSmoothValue(_dataGame.Score);
        _dataGame.EventChangeScore += SetSmoothValue;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeScore -= SetSmoothValue;
    }

}
