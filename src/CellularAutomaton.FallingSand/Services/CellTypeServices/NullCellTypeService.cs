using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    internal sealed class NullCellTypeService : ICellTypeService
    {
        public CellTypeEnum Type => CellTypeEnum.Null;

        public Color Color => throw new NotImplementedException();

        public bool ValidInput => false;

        public bool Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices)
        {
            throw new NotImplementedException();
        }
    }
}
