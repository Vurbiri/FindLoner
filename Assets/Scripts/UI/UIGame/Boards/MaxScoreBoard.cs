
public class MaxScoreBoard : ABoard
{
    private void Start()
    {
        SetText(_dataGame.MaxScore.ToString());
        _dataGame.EventChangeMaxScore += SetText;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeMaxScore -= SetText;
    }
}
