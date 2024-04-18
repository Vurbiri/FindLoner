using UnityEngine;

public class GameLevelSetupData 
{
    public int Size { get; }
    public int CountShapes { get; }
    public int Count { get; }
    public bool IsMonochrome { get; }

    public GameLevelSetupData(int size, int countTypes, bool isMonochrome)
    {
        Size = size;
        CountShapes = size * size;
        Count = countTypes;
        IsMonochrome = isMonochrome;
    }

    public GameLevelSetupData()
    {
        Size = Random.Range(2, 12);
        CountShapes = Size * Size;
        Count = Size;
        IsMonochrome = Random.Range(0, 100) < 50;
    }
}
