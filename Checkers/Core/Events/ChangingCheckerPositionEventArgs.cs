namespace Checkers.Core.GameEventArgs;

public class ChangingCheckerPositionEventArgs
{
    public Position Next { get; private set; }

    public Position Previous { get; private set; }

    public List<Position> Moves { get; private set; }

    public ChangingCheckerPositionEventArgs(Position previous, Position next, List<Position> moves)
    {
        Previous = previous;
        Next = next;
        Moves = moves;
    }
}
