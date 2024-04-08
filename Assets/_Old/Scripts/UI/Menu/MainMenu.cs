using System.Collections;
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

        StartCoroutine(ButtonInitialize());

        #region Local Function
        IEnumerator ButtonInitialize()
        {
            _leaderboard.interactable = _ysdk.IsLeaderboard;
            var wait = _ysdk.CanReview();
            yield return wait;
            _review.interactable = _ysdk.IsLogOn && wait.Result;
        }
        #endregion
    }

    public void OnReview()
    {
        _review.interactable = false;
        _ysdk.RequestReview();
    }
}
