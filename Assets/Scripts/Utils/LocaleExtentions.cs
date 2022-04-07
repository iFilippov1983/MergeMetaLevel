namespace Utils
{
    public static class LocaleExtensions
    {
        public static string Loc(this string str)
        {
            var loc = I2.Loc.LocalizationManager.GetTranslation(str) 
                      ?? $"<{str}>";
            return loc;
        }
        
        public static string Loc(this string str, string prefix)
        {
            var loc = I2.Loc.LocalizationManager.GetTranslation($"{prefix}/{str}") 
                      ?? $"<{prefix}/{str}>";
            return loc;
        } 
    }
}