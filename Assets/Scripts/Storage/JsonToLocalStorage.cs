using System;
using System.Collections.Generic;

sealed public class JsonToLocalStorage : ASaveLoadJsonTo
{
    private string _key;

#if UNITY_EDITOR
    public override bool IsValid => false;
#else
    public override bool IsValid => UtilityJS.IsStorage();
#endif

    public override bool Initialize(string key)
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
            var d = Deserialize<Dictionary<string, string>>(json);

            if (d.Result)
            {
                _saved = d.Value;
                return true;
            }
        }

        _saved = new();
        return false;
    }

    public override bool Save(string key, object data)
    {
        if (!SaveToMemory(key, data))
            return false;

        try
        {
            var json = Serialize(_saved);
            if (UtilityJS.SetStorage(_key, json))
            {
                _dictModified = false;
                return true;
            }
        }
        catch (Exception ex)
        {
            Message.Log(ex.Message);
        }
        return false;
    }
}
