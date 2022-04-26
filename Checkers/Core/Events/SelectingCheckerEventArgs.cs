namespace Checkers.Core.Events;

public class SelectingCheckerEventArgs: EventArgs
{
    public Position Position { get; private set; }

    public List<Position> Moves { get; private set; }

    public SelectingCheckerEventArgs(Position position, List<Position> moves)
    {
        Position = position;
        Moves = moves;
    }
}
