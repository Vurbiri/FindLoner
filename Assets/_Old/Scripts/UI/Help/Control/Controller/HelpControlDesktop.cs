
public class HelpControlDesktop : AHelpControlElement
{
    public override void StateOff()
    {
        _imageOff.SetActive(true);
        _imageOn.SetActive(false);
    }

    public override void StateOn()
    {
        _imageOff.SetActive(false);
        _imageOn.SetActive(true);
    }

}
