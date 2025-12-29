public class SettingsMenu : MenuNavigation
{
    private bool _isSave;

    private void OnEnable()
    {
        _isSave = false;
    }
    private void OnDisable() 
    {
        if (_isSave || SettingsGame.Instance == null || SoundSingleton.Instance == null || MusicSingleton.Instance == null)
            return;

        SettingsGame.Instance.Cancel();
    }

    public void OnOk()
    {
        _isSave = true;
        Message.Saving("GoodSaveSettings", SettingsGame.Instance.Save());
    }
}
