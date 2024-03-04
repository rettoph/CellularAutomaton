using CellularAutomaton.FallingSand.Enums;

namespace CellularAutomaton.FallingSand
{
    public struct CellData
    {
        public static readonly CellData Air = new CellData()
        {
            Type = CellTypeEnum.Air
        };

        public CellTypeEnum Type;
    }
}
