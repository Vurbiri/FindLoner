
public class HelpControlMobile : AHelpControlElement
{
    public override void StateOff()
    {
        _imageOff.SetActive(true);
        _imageOn.SetActive(false);
    }

    public override void StateOn()
    {
        _imageOff.SetActive(true);
        _imageOn.SetActive(true);
    }
}
