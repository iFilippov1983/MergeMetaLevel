namespace Data
{
    public enum ResourceType
    {
        Level,
        Xp,
        Coins,
        Diamonds,
        Hearts,
        InfiniteHearts,
        
        Music = 20,
        Sound,
        Vibro,
        
        PrelevelBooster1 = 30,
        PrelevelBooster2,
        PrelevelBooster3,
        
        GameBooster1 = 40,
        GameBooster2,
        GameBooster3,
        
    }

    public enum PrelevelBoosterType
    {
        PrelevelBooster1 = ResourceType.PrelevelBooster1,
        PrelevelBooster2 = ResourceType.PrelevelBooster2,
        PrelevelBooster3 = ResourceType.PrelevelBooster3,
    }
    
    public enum GameBoosterType
    {
        GameBooster1 = ResourceType.GameBooster1,
        GameBooster2 = ResourceType.GameBooster2,
        GameBooster3 = ResourceType.GameBooster3,
    }

    public enum CharacterType
    {
        Ann,
        Alex,
        Ann_12,
        Ann_14
    }
    
}