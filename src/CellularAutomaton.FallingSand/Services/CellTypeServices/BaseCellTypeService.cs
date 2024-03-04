using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    internal abstract class BaseCellTypeService : ICellTypeService
    {
        public CellTypeEnum Type { get; }

        public BaseCellTypeService(CellTypeEnum type)
        {
            this.Type = type;
        }

        public abstract void Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer vertices);

        protected void Swap(ref Cell<CellData> cellA, ref CellData latestCellAData, ref Cell<CellData> cellB, VertexCellBuffer vertices)
        {
            cellA.New = cellB.Latest;
            cellA.Asleep = false;

            cellB.New = latestCellAData;
            cellB.Asleep = false;

            vertices.Swap(cellA.Index, cellB.Index);
        }
    }
}
