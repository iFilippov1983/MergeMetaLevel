using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    internal abstract class ContentData
    {
        public abstract ContentType GetContentType();
    }
}
