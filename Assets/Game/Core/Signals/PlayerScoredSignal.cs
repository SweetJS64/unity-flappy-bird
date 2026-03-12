namespace Game.Core.Signals
{
    public sealed class PlayerScoredSignal
    {
        public int Value { get; }
        public PlayerScoredSignal(int value = 1) => Value = value;
    }
}
