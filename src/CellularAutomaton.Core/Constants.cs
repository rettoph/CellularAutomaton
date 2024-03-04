using Guppy.Game.MonoGame.Graphics.Effects;
using Guppy.Resources;

namespace CellularAutomaton.Core
{
    public static class Constants
    {
        public static class Packs
        {
            public static readonly string Default = Path.Combine("Content", "Default");
        }

        public static class Resources
        {
            public static class EffectCodes
            {
                public static readonly Resource<EffectCode> Grid = Resource.Get<EffectCode>($"{nameof(EffectCode)}.{nameof(Grid)}");
            }
        }
    }
}
