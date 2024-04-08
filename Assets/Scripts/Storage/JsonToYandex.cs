using System;
using System.Collections;
using System.Collections.Generic;

public class JsonToYandex : ASaveLoadJsonTo
{
    private string _key;
    private YandexSDK _ysdk;

    public override bool IsValid => _ysdk.IsLogOn;

    public override IEnumerator InitializeCoroutine(string key, Action<bool> callback)
    {
        _key = key;
        _ysdk = YandexSDK.Instance;

        WaitResult<string> waitResult;
        string json;

        yield return (waitResult = _ysdk.Load(_key));
        json = waitResult.Result;

        if (!string.IsNullOrEmpty(json))
        {
            Return<Dictionary<string, string>> d = Deserialize<Dictionary<string, string>>(json);

            if (d.Result)
            {
                _saved = d.Value;
                callback?.Invoke(true);
            }
        }

        _saved = new();
        callback?.Invoke(false);
    }

    public override IEnumerator SaveCoroutine(string key, object data, bool isSaveHard, Action<bool> callback)
    {
        bool result;
        if (!((result = SaveSoft(key, data)) && isSaveHard && _dictModified))
        {
            callback?.Invoke(result);
            yield break;
        }

        yield return SaveToFileCoroutine();

        #region Local Function
        IEnumerator SaveToFileCoroutine()
        {
            WaitResult<bool> waitResult;
            yield return (waitResult = _ysdk.Save(_key, Serialize(_saved)));
            _dictModified = !waitResult.Result;
            callback?.Invoke(waitResult.Result);
        }
        #endregion
    }
}
