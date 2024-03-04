namespace CellularAutomaton.FallingSand.Enums
{
    [Flags]
    public enum CellTypeEnum
    {
        Null = 0,
        Air = 1 << 0,
        Sand = 1 << 1,
        Water = 1 << 2,
        Cement = 1 << 3
    }
}
