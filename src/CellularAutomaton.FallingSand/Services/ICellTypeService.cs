using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Guppy.Attributes;
using Guppy.Enums;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services
{
    [Service<ICellTypeService>(ServiceLifetime.Scoped, true)]
    public interface ICellTypeService
    {
        Color Color { get; }

        CellTypeEnum Type { get; }

        bool ValidInput { get; }

        bool Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices);
    }
}
