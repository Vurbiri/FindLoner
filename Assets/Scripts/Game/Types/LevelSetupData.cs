using UnityEngine;

public class LevelSetupData 
{
    public float Time { get; }
    public int Size { get; }
    public int CountShapes { get; }
    public int Count { get; }
    public bool IsMonochrome { get; }
    public Increment Range { get; }
    public int CountShuffle { get; }

    public LevelSetupData(float startTime, int size, int countTypes, bool isMonochrome, int countShuffle)
    {
        Time = startTime;
        Size = size;
        CountShapes = size * size;
        Count = countTypes;
        IsMonochrome = isMonochrome;
        Range = null;
        CountShuffle = countShuffle;
    }
    public LevelSetupData(float startTime, int size, int attempts, bool isMonochrome, int countShuffle, Increment range) : this(startTime, size, attempts, isMonochrome, countShuffle)
    {
        Range = range;
    }
}
