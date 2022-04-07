
using UnityEngine;

public static class MenuExtensions
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Clear Profile")]
    public static void ClearProfile() 
        => PlayerPrefs.SetString("data", null);
#endif
}