#if UNITY_EDITOR

using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class YandexSDK
{
    [Space]
    public bool _isDesktop = true;
    public bool _isLogOn = true;
    [Dropdown("GetLangValues")] public string _lang = "ru";
    public bool _isInitialize = true;
    public bool _isLeaderboard = true;
    public bool _isPlayer = true;

    private DropdownList<string> GetLangValues()
    {
        return new DropdownList<string>()
        {
            { "�������",  "ru" },
            { "English",  "en" }
        };
    }

    public bool IsDesktop => _isDesktop;

    public bool IsInitialize => _isInitialize;
    public bool IsPlayer => IsInitialize && _isPlayer;
    public bool IsLeaderboard => IsLogOn && _isLeaderboard;
    public string PlayerName => "PlayerName";
    public bool IsLogOn => _isLogOn;
    public string Lang => _lang;

    public UniTask<bool> InitYsdk() => UniTask.RunOnThreadPool(() => _isInitialize);
    public void LoadingAPI_Ready() { }
    public UniTask<bool> InitPlayer() => UniTask.RunOnThreadPool(() => _isPlayer);
    public async UniTask<bool> LogOn()
    {
        await UniTask.Delay(1000, true);
        _isLogOn = true;
        return true;
    }
    public UniTask<bool> InitLeaderboards() => UniTask.RunOnThreadPool(() => IsLeaderboard);
    public string GetPlayerAvatarURL(AvatarSize size) => string.Empty;

    public UniTask<Return<PlayerRecord>> GetPlayerResult() => UniTask.RunOnThreadPool(() => new Return<PlayerRecord>(new PlayerRecord(6, 1)));
    public UniTask<bool> SetScore(long score) => UniTask.RunOnThreadPool(() => true);
    public UniTask<Return<Leaderboard>> GetLeaderboard(int quantityTop, bool includeUser = false, int quantityAround = 0, AvatarSize size = AvatarSize.Small)
    {
        Debug.Log(_lbName);

        List<LeaderboardRecord> list = new()
        {
            new(1, 1100, "����� ������", ""),
            new(2, 1000, "�������� �������", "https://pixelbox.ru/wp-content/uploads/2021/10/dark-avatar-vk-pixelbox.ru-87.jpg"),
            new(3, 900, "������ ������", "������ ������"),
            new(4, 800, "����� Ը���", ""),
            new(5, 600, "������ ����", ""),
            new(6, 550, "�������� ����", ""),
            new(8, 500, "", ""),
            new(9, 400, "�������� ����", ""),
            new(10, 300, "�������� �������", "https://pixelbox.ru/wp-content/uploads/2021/10/dark-avatar-vk-pixelbox.ru-7-150x150.png"),
            new(11, 200, "������� �����", ""),
            new(12, 100, "������� ����", "")
        };

        Leaderboard l = new(2, list.ToArray());

        return UniTask.RunOnThreadPool(() => new Return<Leaderboard>(l));
    }

    public UniTask<Return<Leaderboard>> GetLeaderboardTest()
    {
        List<LeaderboardRecord> list = new()
        {
            new(1, 1100, "����� ������", ""),
            new(2, 1000, "�������� �������", ""),
            new(3, 900, "������ ������", ""),
            new(4, 800, "����� Ը���", ""),
            new(5, 600, "������ ����", ""),
            new(6, 550, "�������� ����", ""),
            new(7, 500, "", ""),
            new(9, 400, "�������� ����", ""),
            new(10, 300, "�������� �������", ""),
        };

        Leaderboard l = new(2, list.ToArray());

        return UniTask.RunOnThreadPool(() => new Return<Leaderboard>(l));
    }

    public async UniTask<bool> Save(string key, string data)
    {
        using StreamWriter sw = new(Path.Combine(Application.persistentDataPath, key));
        await sw.WriteAsync(data);

        return true;
    }
    public async UniTask<string> Load(string key)
    {
        string path = Path.Combine(Application.persistentDataPath, key);
        if (File.Exists(path))
        {
            using StreamReader sr = new(path);
            return await sr.ReadToEndAsync();
        }
        return null;
    }

    public UniTask<bool> CanReview() => UniTask.RunOnThreadPool(() => _isLogOn);
    public async UniTaskVoid RequestReview() => await UniTask.Delay(1); 

    public UniTask<bool> CanShortcut() => UniTask.RunOnThreadPool(() => _isLogOn);
    public UniTask<bool> CreateShortcut() => UniTask.RunOnThreadPool(() => _isLogOn);

}
#endif
