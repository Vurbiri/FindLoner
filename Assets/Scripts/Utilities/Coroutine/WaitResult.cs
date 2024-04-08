using UnityEngine;

public class WaitResult<T> : CustomYieldInstruction
{
    public T Result { get; private set; }

    public override bool keepWaiting => _keepWaiting;
    public bool _keepWaiting = true;

    public static WaitResult<T> Empty { get; } = new(default);

    public WaitResult()
    {
        _keepWaiting = true;
    }
    public WaitResult(T result)
    {
        Result = result;
        _keepWaiting = false;
    }

    public void SetResult(T result)
    {
        Result = result;
        _keepWaiting = false;
    }

    public WaitResult<T> Delete()
    {
        Result = default;
        _keepWaiting = false;

        return new();
    }

}
