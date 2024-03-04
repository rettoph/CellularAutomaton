using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Guppy.Attributes;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    [AutoLoad]
    internal sealed class AirCellTypeService : BaseCellTypeService
    {
        public AirCellTypeService() : base(CellTypeEnum.Air, Color.White, true)
        {
        }


        public override bool Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices)
        {
            if (base.Update(ref cell, ref latest, ref grid, vertices) == true)
            {
                return true;
            }

            if (cell.IdleCount >= 5)
            {
                cell.Asleep = true;
            }

            return false;
        }
    }
}
