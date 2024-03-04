using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace CellularAutomaton.Core.Graphics.Vertices
{
    [StructLayout(LayoutKind.Explicit)]
    public struct VertexIndex : IVertexType
    {
        private static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        [FieldOffset(0)]
        public int Index;
    }
}
