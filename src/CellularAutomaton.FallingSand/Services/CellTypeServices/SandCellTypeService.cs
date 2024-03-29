﻿using CellularAutomaton.Core;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using Guppy.Attributes;
using Microsoft.Xna.Framework;

namespace CellularAutomaton.FallingSand.Services.CellTypeServices
{
    [AutoLoad]
    internal class SandCellTypeService : BaseGravityCellTypeService
    {
        public SandCellTypeService() : base(CellTypeEnum.Sand, CellTypeEnum.Water, Color.SandyBrown, true)
        {
        }

        public override bool Update(ref Cell<CellData> cell, ref CellData latest, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices)
        {
            if (base.Update(ref cell, ref latest, ref grid, vertices))
            {
                return true;
            }

            if (cell.IdleCount >= 100)
            {
                cell.Asleep = true;
            }

            return false;
        }
    }
}
