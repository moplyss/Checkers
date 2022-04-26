namespace Checkers.Core;

public class Position
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Position position1, Position position2) =>
        position1.X == position2.X && position1.Y == position2.Y;

    public static bool operator !=(Position position1, Position position2) =>
        position1.X != position2.X && position1.Y != position2.Y;

    public override bool Equals(object? obj) =>
        obj is not null && this == (Position)obj;

    public override int GetHashCode() => 
        HashCode.Combine(X, Y);

    public override string ToString() => "Y: " + Y;
}