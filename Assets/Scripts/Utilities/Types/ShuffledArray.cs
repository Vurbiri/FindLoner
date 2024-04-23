using UnityEngine;

public class ShuffledArray<T> 
{
    private const int CAPACITY_DEFAULT = 8;

    private T[] _array;
    private int _size, _count, _cursor, _capacity = CAPACITY_DEFAULT;

    public T this[int i] { set => _array[i] = value; }
    public T this[int i, int j] { get => _array[i * _size + j]; set => _array[i * _size + j] = value; }
    public T Next => _array[_cursor++];

    public int Size { get => _size; set => _size = value; }

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
        _size = _capacity / 2;
        _array = array;
    }

    public void ReSize(int size)
    {
        if (_size == size) return;

        _size = size;
        _count = size * size;
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
        if (_cursor < _count)
        {
            value = _array[_cursor++];
            return true;
        }

        value = default;
        return false;
    }
}
