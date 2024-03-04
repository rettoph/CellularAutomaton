namespace CellularAutomaton.FallingSand.Enums
{
    [Flags]
    public enum CellTypeEnum
    {
        Null = 0,
        Air = 1 << 0,
        Sand = 1 << 2,
    }
}
