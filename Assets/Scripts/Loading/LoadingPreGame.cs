using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPreGame : MonoBehaviour
{
    [SerializeField] private string _keySave = "FDL";
    [Space]
    [SerializeField, Scene] private int _sceneDesktop = 0;
    [SerializeField, Scene] private int _sceneMobile = 0;
    [Space]
    [SerializeField] private Slider _slider;

    private void Start()
    {
        Message.Log("Start LoadingPreGame");

        Random.InitState((int)(System.DateTime.Now.Ticks - System.DateTime.UnixEpoch.Ticks));

        var settings = SettingsGame.InstanceF;
		settings.SetPlatform();

		var loadScene = new LoadScene(settings.IsDesktop ? _sceneDesktop : _sceneMobile, _slider, this);

		var localization = Localization.InstanceF;
		if (!localization.Initialize())
			Message.Error("Error loading Localization!");

        Banners.InstanceF.Initialize();

        if (!Storage.StoragesCreate())
            Message.Banner(localization.GetText("ErrorStorage"), MessageType.Error, 7000);

        settings.IsFirstStart = !InitializeStorages(_keySave);

        Message.Log("End LoadingPreGame");
        loadScene.End();

        #region Local Functions
        static bool InitializeStorages(string key)
        {
            bool isLoad = Storage.Initialize(key);

            if (isLoad)
                Message.Log("Storage initialize");
            else
                Message.Log("Storage not initialize");

            return SettingsGame.Instance.Initialize(isLoad) | DataGame.InstanceF.Initialize(isLoad);
        }
        #endregion
    }
}
