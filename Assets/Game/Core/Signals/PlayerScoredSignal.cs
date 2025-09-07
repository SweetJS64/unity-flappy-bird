namespace Game.Core.Signals
{
    public sealed class PlayerScoredSignal
    {
        public readonly int Value;
        public PlayerScoredSignal(int value = 1) => Value = value;
    }
}
