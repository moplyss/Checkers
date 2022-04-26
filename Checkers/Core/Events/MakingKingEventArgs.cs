namespace Checkers.Core.GameEventArgs;

public class MakingKingEventArgs
{
    public Position Position { get; private set; }

    public MakingKingEventArgs(Checker checker)
    {
        Position = checker.Position;
    }
}
