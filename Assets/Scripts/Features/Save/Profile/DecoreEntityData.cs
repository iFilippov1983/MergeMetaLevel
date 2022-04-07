using System;

namespace Data
{
    [Serializable]
    public class DecoreEntityData
    {
        public int SkinIndex;

        public void Set(DecoreEntityData template)
        {
            SkinIndex = template.SkinIndex;
        }
    }
}