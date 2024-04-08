using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPreGame : MonoBehaviour
{
    [SerializeField] private string _keySave = "FDL_test";
    [Space]
    [SerializeField] private Slider _slider;
    [SerializeField] private LogOnPanel _logOnPanel;

    private const int NEXT_SCENE = 1;

    private void Start() => StartCoroutine(LoadingCoroutine());

    private IEnumerator LoadingCoroutine()
    {
        Message.Log("Start LoadingPreGame");
        
        LoadScene loadScene = new(NEXT_SCENE, _slider, true);
        StartCoroutine(loadScene.StartCoroutine());

        YandexSDK ysdk = YandexSDK.InstanceF;
        Localization localization = Localization.InstanceF;
        SettingsGame settings = SettingsGame.InstanceF;

        if (!localization.Initialize())
            Message.Error("Error loading Localization!");
        
        ProgressLoad(0.1f);

        yield return StartCoroutine(InitializeYSDKCoroutine());

        ProgressLoad(0.18f);

        settings.SetPlatform();
        Banners.InstanceF.Initialize();
        PoolBlocks.InstanceF.Initialize();

        ProgressLoad(0.28f);

        yield return StartCoroutine(CreateStoragesCoroutine());

        if (!ysdk.IsLogOn)
        {
            yield return StartCoroutine(_logOnPanel.TryLogOnCoroutine());
            if (ysdk.IsLogOn)
                yield return StartCoroutine(CreateStoragesCoroutine());
        }

        Message.Log("End LoadingPreGame");
        //loadScene.End();

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
                Message.Banner(localization.GetText("ErrorStorage"), MessageType.Error, 7000);
            
            ProgressLoad(0.35f);

            yield return StartCoroutine(InitializeStoragesCoroutine());

            ProgressLoad(0.43f);

            #region Local Functions
            IEnumerator InitializeStoragesCoroutine()
            {
                bool isLoad = false;
                yield return StartCoroutine(Storage.InitializeCoroutine(_keySave, (b) => isLoad = b));
            
                if (isLoad)
                    Message.Log("Storage initialize");
                else
                    Message.Log("Storage not initialize");

                settings.IsFirstStart = !Load(isLoad);

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
