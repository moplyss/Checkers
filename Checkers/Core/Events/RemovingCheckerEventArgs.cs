namespace Checkers.Core.GameEventArgs;

public class RemovingCheckerEventArgs
{
    public Position Position { get; private set; }

    public RemovingCheckerEventArgs(Position position)
    {
        Position = position;
    }
}
