using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Guppy.Attributes;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    [AutoLoad]
    internal class CementCellType : BaseCellTypeService
    {
        public CementCellType() : base(CellTypeEnum.Cement, Color.Gray, true)
        {
        }

        public override bool Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices)
        {
            base.Update(ref cell, ref latest, ref grid, vertices);

            if (cell.IdleCount >= 1)
            {
                cell.Asleep = true;
            }

            return false;
        }
    }
}
