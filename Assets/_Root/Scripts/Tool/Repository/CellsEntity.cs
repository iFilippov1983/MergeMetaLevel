using Data;
using Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    internal class CellsEntity : Repository<int, CellView, CellProperties>
    {
        public CellsEntity(IEnumerable<CellProperties> properties) : base(properties)
        {
        }

        protected override CellView CreateItem(CellProperties properties) 
            => properties.CellView;

        protected override int GetKey(CellProperties properties) 
            => properties.Number;
    }
}
