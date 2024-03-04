using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    internal sealed class NotImplementedCellTypeService : ICellTypeService
    {
        public CellTypeEnum Type => throw new NotImplementedException();

        public void Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer vertices)
        {
            throw new NotImplementedException();
        }
    }
}
