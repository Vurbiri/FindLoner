
public class Increment
{
    private int _value, _count;

    public int Count => _count;
    public int Next => ++_value;
    public int Current => _value;

    public Increment(int count) 
    {  
        _value = 1; 
        _count = count;
    }

    public Increment(int value, int count)
    {
        _value = value;
        _count = count;
    }

    public bool TryGetNext(out int value) 
    { 
        if(_count >= 0) 
        {
            value = _count == 0 ? 0 : _value++;
            _count--;
            return true;
        }

        value = 0;
        return false;
    }
}
