using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Guppy.Attributes;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    [AutoLoad]
    internal class SandCellTypeService : BaseCellTypeService
    {
        public SandCellTypeService() : base(CellTypeEnum.Sand)
        {
        }

        public override void Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer vertices)
        {
            ref Cell<CellData> below = ref grid.GetCell(cell.Position.X, cell.Position.Y + 1, out bool belowExists);
            if (belowExists && below.Latest.Type == CellTypeEnum.Air)
            {
                this.Swap(ref cell, ref latest, ref below, vertices);
            }

            if (cell.IdleCount >= 10)
            {
                cell.Asleep = true;
            }

            vertices.Set(cell.Index, Color.SandyBrown);
        }
    }
}
