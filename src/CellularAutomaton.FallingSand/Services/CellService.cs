using CellularAutomaton.Core;
using CellularAutomaton.Core.Services;
using CellularAutomaton.Core.Utilities;
using CellularAutomaton.FallingSand.Enums;
using CellularAutomaton.FallingSand.Services.CellTypeServices;
using System.Runtime.InteropServices;

namespace CellularAutomaton.FallingSand.Services
{
    internal sealed class CellService : ICellService<CellData>
    {
        private readonly Dictionary<CellTypeEnum, ICellTypeService> _cellTypeServices;

        public ICellTypeService this[CellTypeEnum type] => _cellTypeServices[type];

        public CellService(NullCellTypeService defaultCellTypeService, IEnumerable<ICellTypeService> cellTypeServices)
        {
            _cellTypeServices = cellTypeServices.ToDictionary(x => x.Type, x => x);

            foreach (CellTypeEnum cellType in Enum.GetValues<CellTypeEnum>())
            {
                ref ICellTypeService? cellTypeService = ref CollectionsMarshal.GetValueRefOrAddDefault(_cellTypeServices, cellType, out bool exists);
                if (exists == false)
                {
                    cellTypeService = defaultCellTypeService;
                }
            }
        }

        ICellTypeService Get(CellTypeEnum type)
        {
            return _cellTypeServices[type];
        }

        public void Update(ref Cell<CellData> cell, ref Grid<CellData> grid, VertexCellBuffer<CellData> vertices)
        {
            ref CellData latest = ref cell.Latest;
            if (_cellTypeServices[latest.Type].Update(ref cell, ref latest, ref grid, vertices) == false)
            {
                vertices.Update(ref cell);
            }
        }

        public IEnumerable<ICellTypeService> GetAll()
        {
            return _cellTypeServices.Values;
        }
    }
}
