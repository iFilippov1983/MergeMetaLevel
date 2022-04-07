using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelConfig_Old
{
    public int width, height;
    public int number;
    public List<int> score;
    public List<LevelBlockConfig> blocks;
    public List<LevelBoardConfig> board;
    public List<LevelTargetConfig> targets;
    public List<LevelCoverConfig> covers;
    public List<LevelLockerConfig> lockers;
    public Dictionary<string, ILevelGeneratorConfig> generators;
}

public static class LevelConfigExtensions
{
    public static LevelBlockConfig GetChip(this LevelConfig_Old config, int x, int y) 
        => config.blocks.Find(v => v.x -1 == x & v.y-1 == y);
    public static bool HasChip(this LevelConfig_Old config, int x, int y) 
        => config.GetChip(x, y) != null;
    public static bool HasEmpty(this LevelConfig_Old config, int x, int y) 
        => config.GetChip(x, y)?.block == "empty";
    public static LevelBoardConfig GetBoardConfig(this LevelConfig_Old config, int x, int y) 
        => config.board.Find(v => v.x -1 ==x && v.y -1 == y);
    public static bool HasGeneratorOnStart(this LevelConfig_Old config, int x, int y) 
        => GetBoardConfig(config, x,y)?.generator_on_start != null;
    public static bool HasGeneratorOnGoing(this LevelConfig_Old config, int x, int y) 
        => GetBoardConfig(config, x,y)?.generator_on_going != null;
}

public interface ILevelGeneratorConfig{};

public class OnGoingLevelGeneratorConfig : Dictionary<string, int> , ILevelGeneratorConfig {}

public class OnQueueLevelGeneratorConfig : ILevelGeneratorConfig
{
    public bool looped;
    public List<string> list;
}

public class TargetLevelGeneratorConfig : ILevelGeneratorConfig
{
    public string block;
    public int min_count;
    public int max_count;
    public int generate_count;
    public int drop_rate;
}

[Serializable]
public class LevelLockerConfig
{
    public int x, y;
    public string block;
}

[Serializable]
public class LevelCoverConfig
{
    public LevelTargetConfig target;
    public string block;
    public List<Vector2> cells;
}

[Serializable]
public class LevelBlockConfig
{
    public int x, y;
    public int lives;
    public string block;

    public static LevelBlockConfig Create(int xx, int yy, string name)
    {
        return new LevelBlockConfig()
        {
            x = xx,
            y = yy,
            block = name,
        };
    }

    public LevelBlockConfig Clone()
        => new LevelBlockConfig()
        {
            x = x,
            y = y,
            lives =  lives,
            block = block,
        };
}

[Serializable]
public class LevelBoardConfig
{
    public int x, y;
    public LevelBoardDirection direction;
    public string generator_on_going; // on_queue_1 to 7
    public string generator_on_start;
}
public enum LevelBoardDirection
{
    top,
}

[Serializable]
public class LevelTargetConfig
{
    public int count;
    public int position;
    public string block;
}