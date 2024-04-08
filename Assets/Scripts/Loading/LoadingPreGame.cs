using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPreGame : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private LogOnPanel _logOnPanel;

    private const int NEXT_SCENE = 1;

    private void Start() => Loading().Forget();

    private async UniTaskVoid Loading()
    {
        Message.Log("Start LoadingPreGame");
        
        LoadScene loadScene = new(NEXT_SCENE, _slider, true);
        loadScene.Start();

        YandexSDK ysdk = YandexSDK.InstanceF;
        Localization localization = Localization.InstanceF;
        SettingsGame settings = SettingsGame.InstanceF;

        if (!localization.Initialize())
            Message.Error("Error loading Localization!");
        
        ProgressLoad(0.1f);

        if (!await InitializeYSDK())
            Message.Log("YandexSDK - initialization error!");

        ProgressLoad(0.18f);

        settings.SetPlatform();
        Banners.InstanceF.Initialize();
        PoolBlocks.InstanceF.Initialize();

        ProgressLoad(0.28f);

        await CreateStorages();
                
        if (!ysdk.IsLogOn)
        {
            if (await _logOnPanel.TryLogOn())
                await CreateStorages();
        }

        Message.Log("End LoadingPreGame");
        loadScene.End();

        #region Local Functions
        async UniTask<bool> InitializeYSDK()
        {
            if (!await ysdk.InitYsdk())
                return false;

            if (!await ysdk.InitPlayer())
                Message.Log("Player - initialization error!");

            if (!await ysdk.InitLeaderboards())
                Message.Log("Leaderboards - initialization error!");

            return true;
        }
        async UniTask CreateStorages(string key = null)
        {
            if (!Storage.StoragesCreate())
                Message.Banner(localization.GetText("ErrorStorage"), MessageType.Error, 7000);
            
            ProgressLoad(0.35f);

            settings.IsFirstStart = !await InitializeStorages();

            ProgressLoad(0.43f);

            #region Local Functions
            async UniTask<bool> InitializeStorages()
            {
                bool isLoad = await Storage.Initialize(key);
            
                if (isLoad)
                    Message.Log("Storage initialize");
                else
                    Message.Log("Storage not initialize");

                return Load(isLoad);

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
