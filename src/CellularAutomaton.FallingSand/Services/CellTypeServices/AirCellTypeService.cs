using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Guppy.Attributes;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    [AutoLoad]
    internal sealed class AirCellTypeService : ICellTypeService
    {
        public CellTypeEnum Type => CellTypeEnum.Air;

        public void Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer vertices)
        {
            if (cell.IdleCount >= 5)
            {
                cell.Asleep = true;
            }

            vertices.Set(cell.Index, Color.White);
        }
    }
}
