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

    public LevelSetupData(float startTime, int size, int countTypes, bool isMonochrome)
    {
        Time = startTime;
        Size = size;
        CountShapes = size * size;
        Count = countTypes;
        IsMonochrome = isMonochrome;
        Range = null;
        CountShuffle = 0;
    }
    public LevelSetupData(float startTime, int size, int attempts, bool isMonochrome, Increment range, int countShuffle = 0) : this(startTime, size, attempts, isMonochrome)
    {
        Range = range;
        CountShuffle = countShuffle;
    }

    //public LevelSetupData()
    //{
    //    Time = 60f;
    //    Size = Random.Range(2, 12);
    //    CountShapes = Size * Size;
    //    Count = Size;
    //    IsMonochrome = Random.Range(0, 100) < 50;
    //    Range = null;
    //    CountShuffle = 0;
    //}

    //public LevelSetupData(BonusLevelTypes type) : this(60f, Random.Range(4, 11), type == BonusLevelTypes.Single ? 3 : 9, Random.Range(0, 100) < 50)
    //{
    //    Range = new(Random.Range(Size, (type == BonusLevelTypes.Single ? CountShapes / 3 : CountShapes / 2)) + 1);
    //    CountShuffle = Random.Range(0, CountShapes / 2);
    //}
}
