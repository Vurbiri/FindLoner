public class SettingsMenu : MenuNavigation
{
    private SettingsGame _settings;
    private bool isSave;

    protected override void Awake()
    {
        base.Awake();
        _settings = SettingsGame.InstanceF;
    }

    private void OnEnable()
    {
        isSave = false;
    }
    private void OnDisable() 
    {
        if (isSave || SettingsGame.Instance == null)
            return;

        _settings.Cancel();
    }

    public void OnOk()
    {
        isSave = true;

        _settings.Save((b) => Message.Saving("GoodSaveSettings", b));
    }
}
