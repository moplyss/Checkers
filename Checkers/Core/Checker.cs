using System.Diagnostics;

namespace Checkers.Core;

[DebuggerDisplay("{Position.X}, {Position.Y} ------ {Color} ------ {UnderAttackCheckers.Count}")]
public class Checker
{
    public bool IsKing { get; set; } = false;

    public Color Color { get; private set; }

    public Position Position { get; internal set; }

    public List<Position> Moves { get; internal set; } = new();

    public Dictionary<Checker, List<Position>> UnderAttackCheckers { get; internal set; } = new();

    public Checker(Position position, Color color)
    {
        Position = position;
        Color = color;
    }
}
