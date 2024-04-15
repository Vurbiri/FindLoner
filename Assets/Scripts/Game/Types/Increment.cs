
public class Increment
{
    private int _value, _count;
    private readonly int _step;

    public int Count => _count;

    public Increment(int start, int step, int count) 
    {  
        _value = start - step; 
        _step = step;
        _count = count;
    }

    public bool TryGetNext(out int value) 
    { 
        if(_count >= 0) 
        {
            value = _count == 0 ? 0 : _value += _step;
            _count--;
            return true;
        }

        value = 0;
        return false;
    }
}
