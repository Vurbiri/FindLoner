
using UnityEngine;

public class Increment
{
    private int _value;
    private readonly int _step;

    public int Next => Mathf.Clamp(_value += _step, 0 , 99);
    public Increment(int start, int step = 1) 
    {  
        _value = start - step; 
        _step = step;
    }
}
