using UnityEngine;

public class ArrayRandom<T> where T : class, IRandomizeObject
{
    private readonly RandomizeObject[] randomizeObjects;
    private T _oldObject;
    private int _weightMax = 0;
    private int _countMax = 0;

    public T this[int index]{ get => randomizeObjects[index].RandomObject; }
    public int Count => _countMax;

    public T Next
    {
        get
        {
            int randomValue = Random.Range(0, _weightMax);

            foreach (var rObj in randomizeObjects)
            {
                if (rObj.Check(randomValue))
                    return rObj.RandomObject;
            }
            return null;
        }
    }

    public T NextUnique
    {
        get
        {
            T rObject;
            do
            {
                rObject = Next;
                if(rObject != _oldObject)
                {
                    _oldObject = rObject;
                    break;
                }

            }
            while (_countMax > 1);

            return rObject;
        }
    }

    public T[] Range(int countObjects)
    {
        T[] rObjects = new T[countObjects];
        T rObject;
        int count;
        for (int i = 0; i < countObjects; i++)
        {
            do
            {
                rObject = Next;
                count = rObject.MaxCount;

                for (int j = 0; j < i; j++)
                    if (rObject == rObjects[j])
                        count--;

            } while (count <= 0);

            rObjects[i] = rObject;
        }
        return rObjects;
    }

    public ArrayRandom(T[] objects)
    {
        _countMax = objects.Length;
        randomizeObjects = new RandomizeObject[_countMax];

        for (int i = 0; i < _countMax; i++)
        {
            randomizeObjects[i] = new(objects[i], _weightMax);
            _weightMax = randomizeObjects[i].WeightMax;
        }
    }

    public void ReInitialize()
    {
        _weightMax = 0;
        foreach (var obj in randomizeObjects)
            _weightMax = obj.SetWeight(_weightMax);
    }

    public void ReInitialize(int max)
    {
        _countMax = max;
        _weightMax = 0;
        for (int i = 0; i < max; i++)
            _weightMax = randomizeObjects[i].SetWeight(_weightMax);
    }

    #region Nested Classe
    private class RandomizeObject
    {
        public T RandomObject { get; }
        public int WeightMax => WeightMin + Weight;
        public int WeightMin { get; private set; }
        public int Weight => RandomObject.Weight;

        public RandomizeObject(T obj, int weightMin)
        {
            RandomObject = obj;

            WeightMin = weightMin;
        }

        public int SetWeight(int weightMin)
        {
            WeightMin = weightMin;
            return WeightMax;
        }

        public bool Check(int weight) => weight >= WeightMin && weight < WeightMax;
    }
    #endregion
}
