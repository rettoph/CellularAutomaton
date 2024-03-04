using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    internal class BaseLiquidCellTypeService : BaseGravityCellTypeService
    {
        public BaseLiquidCellTypeService(CellTypeEnum type, CellTypeEnum displaceFlags, Color color, bool validInput) : base(type, displaceFlags, color, validInput)
        {
        }

        public override bool Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices)
        {
            if (base.Update(ref cell, ref latest, ref grid, vertices))
            {
                return true;
            }

            int flowDirection = Random.Shared.Next(0, 2) == 0 ? -1 : 1;
            if (this.TryFlowSide(ref cell, ref latest, ref grid, flowDirection, vertices) || this.TryFlowSide(ref cell, ref latest, ref grid, flowDirection * -1, vertices))
            {
                return true;
            }

            return false;
        }

        private bool TryFlowSide(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, int direction, VertexCellBuffer<CellData> vertices)
        {
            for (int i = 1; i < 15; i++)
            {
                ref Cell<CellData> side = ref this.GetNeighbor(ref cell, ref grid, (direction * i), 0, out bool exists);
                if (exists == false)
                {
                    break;
                }

                if (side.Latest.Type == this.Type)
                {
                    continue;
                }

                if (this.CanDisplace(ref cell, ref side))
                {
                    this.Swap(ref cell, ref latest, ref side, vertices);
                    return true;
                }

                break;
            };

            return false;
        }
    }
}
