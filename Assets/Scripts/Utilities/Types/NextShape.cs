using Newtonsoft.Json;
using System.Collections.Generic;

public class NextShape
{
    [JsonIgnore]
    public const int count = 3;
    
    [JsonProperty("i")]
    public int Id { get; }
    [JsonProperty("b")]
    public int[] Blocks { get; }

    [JsonConstructor]
    public NextShape(int id, int[] blocks)
    {
        Id = id;
        Blocks = blocks;
    }

    public NextShape(ShapeType type, List<Block> blocks)
    {
        Id = type.ToInt();
        Blocks = new int[blocks.Count];
        for (int i = 0; i < blocks.Count; i++)
            Blocks[i] = blocks[i].Digit;
    }
}
