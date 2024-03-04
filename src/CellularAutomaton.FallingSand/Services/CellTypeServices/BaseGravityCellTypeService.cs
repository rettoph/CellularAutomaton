using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    internal class BaseGravityCellTypeService : BaseCellTypeService
    {
        public readonly CellTypeEnum DisplaceFlags;
        private readonly CellTypeEnum _thisOrAir;

        public BaseGravityCellTypeService(CellTypeEnum type, CellTypeEnum displaceFlags, Color color, bool validInput) : base(type, color, validInput)
        {
            _thisOrAir = this.Type | CellTypeEnum.Air;
            this.DisplaceFlags = displaceFlags |= CellTypeEnum.Air;
        }

        public override bool Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices)
        {
            base.Update(ref cell, ref latest, ref grid, vertices);

            if (this.TryDisplace(ref cell, ref latest, ref grid, 0, 1, vertices))
            {
                return true;
            }

            int belowSideDirection = Random.Shared.Next(0, 2) == 0 ? 1 : -1;
            if (this.TryDisplace(ref cell, ref latest, ref grid, belowSideDirection, 1, vertices) || this.TryDisplace(ref cell, ref latest, ref grid, belowSideDirection * -1, 1, vertices))
            {
                return true;
            }

            return false;
        }

        protected bool TryDisplace(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, int offsetX, int offsetY, VertexCellBuffer<CellData> vertices)
        {
            ref Cell<CellData> target = ref this.GetNeighbor(ref cell, ref grid, offsetX, offsetY, out bool targetExists);
            if (targetExists)
            {
                return this.TryDisplace(ref cell, ref latest, ref target, vertices);
            }

            cell.New.Type = CellTypeEnum.Air;
            return true;
        }

        protected virtual bool TryDisplace(ref Cell<CellData> cell, ref CellData latest, ref Cell<CellData> target, VertexCellBuffer<CellData> vertices)
        {
            if (this.CanDisplace(ref cell, ref target))
            {
                return this.Swap(ref cell, ref latest, ref target, vertices);
            }

            return false;
        }

        protected virtual bool CanDisplace(ref Cell<CellData> cell, ref Cell<CellData> target)
        {
            if (target.Latest.Type == this.Type)
            {
                return false;
            }

            bool result = this.DisplaceFlags.HasFlag(target.Latest.Type);
            if (target.Updated)
            {
                result &= target.New.Type == CellTypeEnum.Air || target.Old.Type == target.New.Type;
            }

            return result;
        }

        protected virtual ref Cell<CellData> GetNeighbor(ref Cell<CellData> cell, ref Grid<CellData> grid, int offsetX, int offsetY, out bool exists)
        {
            return ref grid.GetCell(cell.Position.X + offsetX, cell.Position.Y + offsetY, out exists);
        }
    }
}
