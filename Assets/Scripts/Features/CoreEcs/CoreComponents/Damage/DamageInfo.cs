public class DamageInfo
{
    public string attacker;
    public bool isChip;
    public bool isExplosion;
    public string match_key;
    public string color;
    public int fromX, fromY;

    public static DamageInfo FromBooster(int x, int y, string attacker)
    {
        return new DamageInfo()
        {
            fromX = x,
            fromY = y,
            attacker = attacker,
            isChip = false,
            isExplosion = true,
            match_key = null,
            color = null,
        };
    }
    
    public static DamageInfo FRom(ChipsEntity e)
    {
        return new DamageInfo()
        {
            // attacker = e.chipConfig.value.name,
            // isExplosion = e.chipConfig.value.isBonus,
            // isChip = e.isChip,
            // match_key = e.chipConfig.value.match_key,
            // color = e.hasColor ? e.color.value : null,
            // fromX = e.position.x,
            // fromY = e.position.y,
        };
    }
}