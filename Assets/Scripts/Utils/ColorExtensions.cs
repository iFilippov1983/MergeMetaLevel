using UnityEngine;

namespace Utils
{
    public static class ColorExtensions
    {
        public static Color NoAlpha(this Color color) 
            => new Color(color.r, color.g, color.b, 0);
        public static Color WithAlpha(this Color color, float value) 
            => new Color(color.r, color.g, color.b, value);
    }
}