using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeStack<T> : IEnumerable<T>
{
    private const int CAPACITY_DEFAULT = 8;
    private T[] _array;
    private int _size, _count, _capacity = CAPACITY_DEFAULT;

    public T this[int i] { get => _array[i]; }
    public T this[int i, int j] { get => _array[i * _size + j]; }
    public T this[Vector2Int v] { get => _array[v.x * _size + v.y]; }

    public int Size { get => _size; set => _size = value; }
    public int Count => _count;

    public FreeStack() => _array = new T[_capacity];
    public FreeStack(int capacity)
    {
       if (capacity > 0)
            _capacity = capacity;

        _array = new T[_capacity];
    }

    public void Push(T item)
    {
        while (_count >= _capacity)
            Array.Resize<T>(ref _array, _capacity <<= 1);

        _array[_count++] = item;
    }

    public T Pop() => _array[--_count];

    public void CopyToShuffledArray(ShuffledArray<T> array)
    {
        array.ReSize(_count);

        for (int i = 0; i < _count; i++)
            array[i] = _array[i];
    }

    public IEnumerator<T> GetEnumerator() => new FreeStackEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => new FreeStackEnumerator(this);
    
    #region Nested class
    private class FreeStackEnumerator : IEnumerator<T>
    {
        private readonly FreeStack<T> _freeStack;
        private int _cursor;
        private T _current;

        public FreeStackEnumerator(FreeStack<T> freeStack)
        {
            _freeStack = freeStack;
            _cursor = -1;
            _current = default;
        }

        public T Current => _current;

        object IEnumerator.Current => _current;

        public bool MoveNext()
        {
            if (++_cursor >= _freeStack.Count)
                return false;

            _current = _freeStack[_cursor];
            return true;
        }

        public void Reset() => _cursor = -1;

        public void Dispose() { }
    }
    #endregion
}
