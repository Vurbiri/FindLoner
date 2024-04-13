using UnityEngine;

public class ShuffledArray<T> 
{
    private const int CAPACITY_DEFAULT = 8;

    private T[] _array;
    private int _count, _cursor, _capacity = CAPACITY_DEFAULT;

    public T this[int i] { set => _array[i] = value; }
    public T Next => _array[_cursor++];

    public ShuffledArray() => _array = new T[_capacity];
    public ShuffledArray(int capacity)
    {
        if (capacity > 0)
            _capacity = capacity;

        _array = new T[_capacity];
    }
    public ShuffledArray(T[] array)
    {
        _capacity = _count = array.Length;
        _array = array;
    }

    public void ReSize(int size)
    {
        if (_count == size) return;

        _count = size;
        if (_count > _capacity)
            _array = new T[_capacity = _count];
    }

    public void Shuffle()
    {
        for (int i = _count - 1, j; i > 0; i--)
        {
            j = Random.Range(0, i + 1);
            (_array[j], _array[i]) = (_array[i], _array[j]);
        }
        _cursor = 0;
    }

    public bool TryGetNext(out T value)
    {
        if (_cursor == _count)
        {
            value = default;
            return false;
        }

        value = _array[_cursor++];
        return true;
    }



    
}
