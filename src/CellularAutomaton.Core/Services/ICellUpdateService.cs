namespace CellularAutomaton.Core.Services
{
    public interface ICellUpdateService<TData>
        where TData : unmanaged
    {
        void UpdateCell(ref Cell<TData> cell, ref Grid<TData> grid);
    }
}
