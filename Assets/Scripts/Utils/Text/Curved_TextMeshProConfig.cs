using UnityEngine;

namespace Utils.Text
{
    public class Curved_TextMeshProConfig : ScriptableObjectSingleton<Curved_TextMeshProConfig>
    {
        public const string FilePath = "Curved_TextMeshProConfig";
        public static Curved_TextMeshProConfig instance => GetInstance(FilePath);
        
        public AnimationCurve Curve;
    }
}