using UnityEngine;

public class BonusLevelSetupData : GameLevelSetupData
{
    public Increment Range { get;}
    public int Time { get; }
    public int CountShuffle { get;}

    public BonusLevelSetupData(int size, int attempts, bool isMonochrome, Increment range, int startTime, int countShuffle = 0) : base(size, attempts, isMonochrome)
    {
        Range = range;
        Time = startTime;
        CountShuffle = countShuffle;
    }

    public BonusLevelSetupData(BonusLevelTypes type) : base(Random.Range(4, 11), type == BonusLevelTypes.Single ?  3 : 9, Random.Range(0, 100) < 50)
    {
        Range = new(1, 1, Random.Range(Size, (type == BonusLevelTypes.Single ? CountShapes / 3 : CountShapes / 2)) + 1);
        Time = 60000;
        CountShuffle = Random.Range(0, CountShapes / 2);
    }
}
