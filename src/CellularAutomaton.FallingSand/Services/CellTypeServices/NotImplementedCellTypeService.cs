using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    internal sealed class NotImplementedCellTypeService : ICellTypeService
    {
        public CellTypeEnum Type => throw new NotImplementedException();

        public Color Color => throw new NotImplementedException();

        public void Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer vertices)
        {
            throw new NotImplementedException();
        }
    }
}
