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

        public bool ValidInput { get; }

        public BaseCellTypeService(CellTypeEnum type, Color color, bool validInput)
        {
            this.Type = type;
            this.Color = color;
            this.ValidInput = validInput;
        }

        public virtual bool Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices)
        {
            if (this.SetColor(ref cell))
            {
                return true;
            }

            return false;
        }

        protected virtual bool SetColor(ref Cell<CellData> cell)
        {
            if (cell.Color == this.Color)
            {
                return false;
            }

            cell.Color = this.Color;
            return true;
        }

        protected bool Swap(ref Cell<CellData> cellA, ref CellData latestCellAData, ref Cell<CellData> cellB, VertexCellBuffer<CellData> vertices)
        {
            Color colorPlaceholder = cellA.Color;
            cellA.New = cellB.Latest;
            cellA.Color = cellB.Color;
            cellA.Asleep = false;
            for (int i = 0; i < cellA.Neighbors.Length; i++)
            {
                cellA.Neighbors[i].Asleep = false;
            }

            cellB.New = latestCellAData;
            cellB.Color = colorPlaceholder;
            cellB.Asleep = false;
            for (int i = 0; i < cellB.Neighbors.Length; i++)
            {
                cellB.Neighbors[i].Asleep = false;
            }

            vertices.Update(ref cellA);
            vertices.Update(ref cellB);

            return true;
        }
    }
}
