using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuNavigation
{
    [Space]
    [SerializeField] protected Button _leaderboard;
    [SerializeField] private Button _review;

    protected YandexSDK _ysdk;

    private void Start()
    {
        _ysdk = YandexSDK.Instance;

        ButtonInitialize().Forget();

        #region Local Function
        async UniTaskVoid ButtonInitialize()
        {
            _leaderboard.interactable = _ysdk.IsLeaderboard;
            _review.interactable = _ysdk.IsLogOn && await _ysdk.CanReview();
        }
        #endregion
    }

    public void OnReview()
    {
        _review.interactable = false;
        _ysdk.RequestReview().Forget();
    }
}
