using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace CellularAutomaton.Core.Graphics.Vertices
{
    [StructLayout(LayoutKind.Explicit)]
    public struct VertexCell : IVertexType
    {
        public static uint ColorMask = 0x00ffffff;
        public static uint AsleepMask = 0x80000000;

        private static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(4, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
        );

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        [FieldOffset(0)]
        public int Index;

        [FieldOffset(4)]
        private uint _data;

        public uint Color
        {
            get => _data & ColorMask;
            set => _data = value & ColorMask;
        }

        public bool Asleep
        {
            set
            {
                if (value)
                {
                    _data |= AsleepMask;
                }
                else
                {
                    _data &= ~AsleepMask;
                }
            }
        }
    }
}
