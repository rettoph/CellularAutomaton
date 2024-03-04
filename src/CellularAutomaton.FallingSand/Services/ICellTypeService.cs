using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Guppy.Attributes;
using Guppy.Enums;

namespace CellularAutomaton.FallingSand.Services
{
    [Service<ICellTypeService>(ServiceLifetime.Scoped, true)]
    public interface ICellTypeService
    {
        CellTypeEnum Type { get; }

        void Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer vertices);
    }
}
