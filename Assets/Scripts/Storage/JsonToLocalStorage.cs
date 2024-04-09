using System;
using System.Collections;
using System.Collections.Generic;

public class JsonToLocalStorage : ASaveLoadJsonTo
{
    private string _key;

    public override bool IsValid => UtilityJS.IsStorage();

    public override IEnumerator InitializeCoroutine(string key, Action<bool> callback)
    {
        _key = key;

        string json;

        try
        {
            json = UtilityJS.GetStorage(_key);
        }
        catch (Exception ex)
        {
            json = null;
            Message.Log(ex.Message);
        }

        if (!string.IsNullOrEmpty(json))
        {
            Return<Dictionary<string, string>> d = Deserialize<Dictionary<string, string>>(json);

            if (d.Result)
            {
                _saved = d.Value;
                callback?.Invoke(true);
                yield break;
            }
        }

        _saved = new();
        callback?.Invoke(false);
    }

    public override IEnumerator SaveCoroutine(string key, object data, Action<bool> callback)
    {
        bool result = SaveToMemory(key, data);
        if (!result)
        {
            callback?.Invoke(false);
            yield break;
        }

        try
        {
            string json = Serialize(_saved);
            result = UtilityJS.SetStorage(_key, json);

        }
        catch (Exception ex)
        {
            result = false;
            Message.Log(ex.Message);
        }
        finally
        {
            callback?.Invoke(result);
        }
    }
}
