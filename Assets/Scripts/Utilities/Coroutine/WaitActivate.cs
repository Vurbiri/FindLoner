using UnityEngine;

public class WaitActivate : CustomYieldInstruction
{
    public override bool keepWaiting => _keepWaiting;
    public bool _keepWaiting = true;

    public WaitActivate()
    {
        _keepWaiting = true;
    }

    public void Activate() => _keepWaiting = false;
    public WaitActivate Deactivate()
    {
        _keepWaiting = true;
        return this;
    }
}
