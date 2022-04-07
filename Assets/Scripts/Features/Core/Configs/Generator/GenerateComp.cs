using System.Collections.Generic;
using Sirenix.OdinInspector;
using Utils;

namespace Data
{
    public abstract class GenerateComp
    {
        public abstract MergeItemConfig Next();
        public virtual bool IsEmpty() => false;
        public abstract GenerateComp Clone();
    }

    public class GenerateOneComp : GenerateComp
    {
        public MergeItemConfig Generated;

        public override MergeItemConfig Next() 
            => Generated;

        public override GenerateComp Clone() => new GenerateOneComp() {Generated = Generated};
    }

    public class CycleGenerateComp : GenerateComp
    {
        public List<MergeItemConfig> Generated = new List<MergeItemConfig>();
        public bool infiniteCount = true;
        [HideIf("@infiniteCount")]
        public int itemsToGenerate = 0;

        private int generated;
        private int index;
        
        public override GenerateComp Clone()
        {
            var res = new CycleGenerateComp()
                {Generated = Generated.Clone(), infiniteCount = infiniteCount, itemsToGenerate = itemsToGenerate};
            res.Generated.Shuffle();
            
            return res;
        }

        public override bool IsEmpty()
            => infiniteCount ? false : generated >= itemsToGenerate;

        public override MergeItemConfig Next()
        {
            var res = Generated[index];
            index = index.IncLoop(Generated.Count);
            ++generated;
            return res;
        }
    }
    public class FromListGenerateComp : GenerateComp
    {
        public List<MergeItemConfig> Generated = new List<MergeItemConfig>();
        
        public override GenerateComp Clone()
        {
            var res = new FromListGenerateComp()
                {Generated = Generated.Clone()};
            res.Generated.Shuffle();
            
            return res;
        }

        public override bool IsEmpty()
            => Generated.Count <= 0;

        public override MergeItemConfig Next()
        {
            var res = Generated[Generated.Count-1];
            Generated.RemoveAt(Generated.Count - 1);
            return res;
        }
    }
}