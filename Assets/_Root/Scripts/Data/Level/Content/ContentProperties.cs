using UnityEditor;
using UnityEngine;

namespace Data
{
    internal abstract class ContentProperties : ScriptableObject
    {
        public abstract ContentType GetContentType();
    }
}