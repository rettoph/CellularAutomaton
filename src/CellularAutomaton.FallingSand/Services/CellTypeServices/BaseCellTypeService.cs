using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    internal abstract class BaseCellTypeService : ICellTypeService
    {
        public CellTypeEnum Type { get; }

        public Color Color { get; }

        public BaseCellTypeService(CellTypeEnum type, Color color)
        {
            this.Type = type;
            this.Color = color;
        }

        public abstract void Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer vertices);

        protected void Swap(ref Cell<CellData> cellA, ref CellData latestCellAData, ref Cell<CellData> cellB, VertexCellBuffer vertices)
        {
            cellA.New = cellB.Latest;
            cellA.Asleep = false;
            for (int i = 0; i < cellA.Neighbors.Length; i++)
            {
                cellA.Neighbors[i].Asleep = false;
            }

            cellB.New = latestCellAData;
            cellB.Asleep = false;
            for(int i=0; i<cellB.Neighbors.Length; i++)
            {
                cellB.Neighbors[i].Asleep = false;
            }


            vertices.Swap(cellA.Index, cellB.Index);
        }
    }
}
