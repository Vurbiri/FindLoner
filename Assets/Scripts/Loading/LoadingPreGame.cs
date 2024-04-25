using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPreGame : MonoBehaviour
{
    [SerializeField] private string _keySave = "FDL_test";
    [Space]
    [SerializeField, Scene] private int _sceneDesktop = 0;
    [SerializeField, Scene] private int _sceneMobile = 0;
    [Space]
    [SerializeField] private Slider _slider;
    [SerializeField] private LogOnPanel _logOnPanel;

    private void Start() => StartCoroutine(LoadingCoroutine());

    private IEnumerator LoadingCoroutine()
    {
        Message.Log("Start LoadingPreGame");

        Random.InitState((int)(System.DateTime.Now.Ticks - System.DateTime.UnixEpoch.Ticks));

        YandexSDK ysdk = YandexSDK.InstanceF;
        Localization localization = Localization.InstanceF;
        SettingsGame settings = SettingsGame.InstanceF;
        LoadScene loadScene = null;

        if (!localization.Initialize())
            Message.Error("Error loading Localization!");
        
        ProgressLoad(0.1f);

        yield return StartCoroutine(InitializeYSDKCoroutine());

        ProgressLoad(0.18f);

        settings.SetPlatform();
        Banners.InstanceF.Initialize();

        loadScene = new(settings.IsDesktop ? _sceneDesktop : _sceneMobile, _slider, true);
        StartCoroutine(loadScene.StartCoroutine());

        ProgressLoad(0.28f);

        yield return StartCoroutine(CreateStoragesCoroutine());

        if (!ysdk.IsLogOn)
        {
            yield return StartCoroutine(_logOnPanel.TryLogOnCoroutine());
            if (ysdk.IsLogOn)
                yield return StartCoroutine(CreateStoragesCoroutine());
        }

        Message.Log("End LoadingPreGame");
        loadScene.End();

        #region Local Functions
        IEnumerator InitializeYSDKCoroutine()
        {
            WaitResult<bool> waitResult;

            yield return (waitResult = ysdk.InitYsdk());
            if (!waitResult.Result)
            {
                Message.Log("YandexSDK - initialization error!");
                yield break;
            }

            yield return (waitResult = ysdk.InitPlayer());
            if (!waitResult.Result)
                Message.Log("Player - initialization error!");

            yield return (waitResult = ysdk.InitLeaderboards());
            if (!waitResult.Result)
                Message.Log("Leaderboards - initialization error!");
        }
        IEnumerator CreateStoragesCoroutine()
        {
            if (!Storage.StoragesCreate())
                Message.Banner(localization.GetText("ErrorStorage"), MessageType.Error, 7f);
            
            ProgressLoad(0.35f);

            yield return StartCoroutine(InitializeStoragesCoroutine());

            ProgressLoad(0.43f);

            #region Local Functions
            IEnumerator InitializeStoragesCoroutine()
            {
                WaitReturnData<bool> waitReturn = new(this);
                yield return waitReturn.Start(Storage.Initialize_Coroutine, _keySave);
            
                if (waitReturn.Return)
                    Message.Log("Storage initialize");
                else
                    Message.Log("Storage not initialize");

                settings.IsFirstStart = !Load(waitReturn.Return);

                #region Local Functions
                bool Load(bool load)
                {
                    bool result = false;

                    result = settings.Initialize(load) || result;
                    return DataGame.InstanceF.Initialize(load) || result;
                }
                #endregion
            }
            #endregion
        }

        void ProgressLoad(float value)
        {
            if (loadScene != null)
                loadScene.SetProgress(value);
            else
                _slider.value = value;
        }
        #endregion
    }

    //private void OnDisable() => YandexSDK.Instance.LoadingAPI_Ready();
}
