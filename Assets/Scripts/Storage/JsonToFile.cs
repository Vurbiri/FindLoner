#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

sealed public class JsonToFile : ASaveLoadJsonTo
{
	private string _path;

	public override bool IsValid => true;

	public override bool Initialize(string fileName)
	{
		_path = Path.Combine(Application.persistentDataPath, fileName);

		if (File.Exists(_path))
		{
			string json;

			using (StreamReader sr = new(_path))
			{
				json = sr.ReadToEnd();
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
			using var sw = new StreamWriter(_path); sw.Write(json);
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