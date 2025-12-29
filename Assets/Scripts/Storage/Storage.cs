using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

public static class Storage
{
    private static ASaveLoadJsonTo service;

    public static Type TypeStorage => service?.GetType();

    public static bool StoragesCreate()
    {
        if (Create<JsonToLocalStorage>())
            return true;

        if (Create<JsonToCookies>())
            return true;

#if UNITY_EDITOR
        if (Create<JsonToFile>())
            return true;

        if (Create<JsonToPlayerPrefs>())
            return true;
#endif

        Create<EmptyStorage>();
        return false;

        #region Local Function
        static bool Create<T>() where T : ASaveLoadJsonTo, new()
        {
            if (typeof(T) == TypeStorage)
                return true;

            service = new T();
            return service.IsValid;
        }
        #endregion
    }
    public static bool Initialize(string key) => service.Initialize(key);
    public static bool Save(string key, object data) => service.Save(key, data);
    public static Return<T> Load<T>(string key) where T : class => service.Load<T>(key);

    public static Return<T> Deserialize<T>(string json) where T : class
    {
        Return<T> result = Return<T>.Empty;
        try
        {
            result = new(JsonConvert.DeserializeObject<T>(json));
        }
        catch (Exception ex)
        {
            Message.Log(ex.Message);
        }

        return result;
    }
    public static string Serialize(object obj) => JsonConvert.SerializeObject(obj);

    public static IEnumerator TryLoadTextureWeb(string url, Action<Return<Texture>> callback)
    {
        if (string.IsNullOrEmpty(url) || !url.StartsWith("https://"))
        {
            callback?.Invoke(Return<Texture>.Empty);
            yield break;
        }

        using (var request = UnityWebRequestTexture.GetTexture(url))
        { 
            yield return request.SendWebRequest();

            if (request.result != Result.Success || request.downloadHandler == null)
            {
                Message.Log("==== UnityWebRequest: " + request.error);
                callback?.Invoke(Return<Texture>.Empty);
                yield break;
            }

            callback?.Invoke(new(((DownloadHandlerTexture)request.downloadHandler).texture));
        }
    }
}
