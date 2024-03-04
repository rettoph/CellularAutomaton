using Guppy.Game.Input;
using Guppy.Messaging;

namespace CellularAutomaton.FallingSand.Messages
{
    internal class RenderAsleepInput : Message<RenderAsleepInput>, IInput
    {
        public readonly bool Value;

        public RenderAsleepInput(bool value)
        {
            Value = value;
        }
    }
}
