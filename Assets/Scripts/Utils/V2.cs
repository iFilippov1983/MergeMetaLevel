using System;

namespace Utils
{
    [Serializable]
    public class V2
    {
        public V2(int xx, int yy)
        {
            x = xx;
            y = yy;
        }
        
        public int x;
        public int y;
    }
}