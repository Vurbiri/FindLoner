using UnityEngine;

public abstract class AHelpControlElement : MonoBehaviour
{
    [SerializeField] protected GameObject _imageOff;
    [SerializeField] protected GameObject _imageOn;
    
    private void Awake()
    {
        _imageOff.SetActive(false);
        _imageOn.SetActive(false);
    }

    public abstract void StateOn();
    public abstract void StateOff();

}
