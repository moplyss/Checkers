namespace Checkers.Core.Events;

public class UnselectingCheckerEventArgs: EventArgs
{
    public Position Position { get; private set; }

    public List<Position> Moves { get; private set; }

    public UnselectingCheckerEventArgs(Position position, List<Position> moves)
    {
        Position = position;
        Moves = moves;
    }
}
