using CellularAutomaton.Core.Utilities;

namespace CellularAutomaton.Core.Services
{
    public interface ICellService<TData>
        where TData : unmanaged
    {
        void Update(ref Cell<TData> cell, ref Grid<TData> grid, VertexCellBuffer<TData> vertices);
    }
}
