using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [Space]
    [SerializeField] private LeaderboardRecordUI _record;
    [SerializeField] private GameObject _recordSeparator;
    [Space]
    [SerializeField] private ScrollRect _rect;
    [Space]
    [SerializeField] private AvatarSize _avatarSize = AvatarSize.Medium;
    [Range(1, 20), SerializeField] private int _maxTop = 20;
    [Range(1, 10), SerializeField] private int _maxAround = 10;

    private YandexSDK YSDK => YandexSDK.Instance;

    private readonly List<LeaderboardRecordUI> _records = new();
    private GameObject _separator;
    RectTransform _recordTransform = null;

    private void OnEnable()
    {
        ScrollToPlayer();
    }

    private void Start()
    {
        if (_rect.content.childCount == 0)
            StartCoroutine(InitializeCoroutine());
    }

    public IEnumerator SetScore(long score)
    {
        if (!YSDK.IsLeaderboard) yield break;

        WaitResult<bool> waitResult = YandexSDK.Instance.SetScore(score);
        yield return waitResult;
        if (!waitResult.Result) yield break;

        if (_rect.content.childCount != 0)
            StartCoroutine(ReInitializeCoroutine());
    }

    private IEnumerator InitializeCoroutine()
    {
        if (!YSDK.IsLeaderboard) yield break;

        Leaderboard leaderboard = null;
        yield return StartCoroutine(GetLeaderboard((l) => leaderboard = l));
        if (leaderboard == null) yield break;

        int userRank = leaderboard.UserRank;

        CreateTable();
        ScrollToPlayer();

        #region Local Functions
        RectTransform CreateTable()
        {
            int preRank = 0;
            bool isPlayer;
            RectTransform content = _rect.content;
            LeaderboardRecordUI recordUI;
            _recordTransform = null;

            foreach (var record in leaderboard.Table)
            {
                if (record.Rank - preRank > 1)
                    _separator = Instantiate(_recordSeparator, content);
                preRank = record.Rank;
                isPlayer = record.Rank == userRank;

                recordUI = Instantiate(_record, content);
                recordUI.Setup(record, isPlayer);
                _records.Add(recordUI);
                if (isPlayer)
                    _recordTransform = recordUI.GetComponent<RectTransform>();
            }

            return _recordTransform;
        }
        #endregion
    }

    private IEnumerator ReInitializeCoroutine()
    {
        if (_rect.content.childCount == 0)
        {
            yield return StartCoroutine(InitializeCoroutine());
            yield break;
        }

        Leaderboard leaderboard = null;
        yield return StartCoroutine(GetLeaderboard((l) => leaderboard = l));
        if (leaderboard == null) yield break;

        int userRank = leaderboard.UserRank;

        CreateTable();
        ScrollToPlayer();

        #region Local Functions
        void CreateTable()
        {
            int preRank = 0;
            bool isPlayer;
            RectTransform content = _rect.content;
            LeaderboardRecordUI recordUI;
            int childCount = _records.Count;
            int i = 0;
            _recordTransform = null;

            if (_separator != null)
            {
                _separator.SetActive(false);
                _separator.transform.SetAsLastSibling();
            }

            foreach (var record in leaderboard.Table)
            {
                isPlayer = record.Rank == userRank;

                if (i < childCount)
                {
                    recordUI = _records[i++];
                }
                else
                {
                    recordUI = Instantiate(_record, content);
                    _records.Add(recordUI);
                }
                recordUI.Setup(record, isPlayer);
                if (isPlayer)
                    _recordTransform = recordUI.GetComponent<RectTransform>();

                if (record.Rank - preRank > 1)
                {
                    if (_separator == null)
                        _separator = Instantiate(_recordSeparator, content);
                    _separator.transform.SetSiblingIndex(recordUI.transform.GetSiblingIndex());
                    _separator.SetActive(true);
                }
                preRank = record.Rank;

            }

            for (; i < childCount; i++)
                _records[i].gameObject.SetActive(false);
        }
        #endregion
    }

    private IEnumerator GetLeaderboard(Action<Leaderboard> action)
    {
        int userRank = 0;

        var waitResult = YSDK.GetPlayerResult();
        yield return waitResult;
        var player = waitResult.Result;
        if (player.Result)
            userRank = player.Value.Rank;

        bool playerInTable = userRank > 0;
        if (playerInTable)
            if (userRank <= (_maxTop - _maxAround))
                playerInTable = false;

        var waitLeaderboard = YSDK.GetLeaderboard(_maxTop, playerInTable, _maxAround, _avatarSize);
        yield return waitLeaderboard;
        var leaderboard = waitLeaderboard.Result;
        if (!leaderboard.Result)
            action?.Invoke(null);

        if (!playerInTable)
            leaderboard.Value.UserRank = userRank;

        action?.Invoke(leaderboard.Value);
    }

    private void ScrollToPlayer()
    {
        if (_recordTransform == null || !gameObject.activeInHierarchy)
            return;

        Canvas.ForceUpdateCanvases();

        RectTransform content = _rect.content;
        RectTransform viewport = _rect.viewport;
        float maxOffset = content.rect.height - viewport.rect.height;
        float offset = -viewport.rect.height / 2f - _recordTransform.localPosition.y;

        if (offset < 0) offset = 0;
        else if (offset > maxOffset) offset = maxOffset;

        content.localPosition = new Vector2(0, offset);
    }

}
