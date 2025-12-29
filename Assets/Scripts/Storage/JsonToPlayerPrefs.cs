#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

sealed public class JsonToPlayerPrefs : ASaveLoadJsonTo
{
	private string _key;

	public override bool IsValid => true;

	public override bool Initialize(string key)
	{
		_key = key;

		if (PlayerPrefs.HasKey(_key))
		{
			string json = PlayerPrefs.GetString(_key);

			if (json != null)
			{
				var d = Deserialize<Dictionary<string, string>>(json);

				if (d.Result)
				{
					_saved = d.Value;
					return true;
				}
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
			PlayerPrefs.SetString(_key, json);
			PlayerPrefs.Save();
            _dictModified = false;
            return true;
        }
		catch (Exception ex)
		{
			Message.Log(ex.Message);
			return false;
		}
	}
}
#endif