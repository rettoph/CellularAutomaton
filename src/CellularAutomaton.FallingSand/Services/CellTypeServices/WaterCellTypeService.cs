using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Guppy.Attributes;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    [AutoLoad]
    internal class WaterCellTypeService : BaseLiquidCellTypeService
    {
        public WaterCellTypeService() : base(CellTypeEnum.Water, CellTypeEnum.Null, Color.Blue, true)
        {
        }

        public override bool Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices)
        {
            if (base.Update(ref cell, ref latest, ref grid, vertices))
            {
                return true;
            }

            if (cell.IdleCount >= 100)
            {
                cell.Asleep = true;
            }

            return false;
        }
    }
}
