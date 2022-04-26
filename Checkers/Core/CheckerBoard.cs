using Checkers.Core.GameEventArgs;

namespace Checkers.Core;

public class CheckerBoard
{
    public readonly static int Size = 8;

    public event EventHandler<MakingKingEventArgs>? MakingKing;
    public event EventHandler<RemovingCheckerEventArgs>? RemovingChecker;
    public event EventHandler<ChangingCheckerPositionEventArgs>? ChangingCheckerPosition;

    private Color[] Colors { get; } = new Color[2];

    public List<Checker> Checkers { get; private set; } = new();

    public CheckerBoard(Color colorPlayer1, Color colorPlayer2)
    {
        Colors[0] = colorPlayer1;
        Colors[1] = colorPlayer2;

        ArrangeCheckers();
    }

    public void RemoveChecker(Checker checker)
    {
        Checkers.Remove(checker);
        OnRemovingChecker(checker);
    }

    public void ChangeCheckerPosition(Checker checker, Position next)
    {
        var prev = checker.Position;
        checker.Position = next;
        MakeKing(checker);

        OnChangingCheckerPosition(checker, prev);
    }

    public Checker? GetChecker(Position position) =>
        Checkers.Find(x => x.Position == position);

    public bool IsCheckerOnPosition(Position position) =>
        Checkers.Exists(x => x.Position == position);

    public bool IsPositionOnBoard(Position position) =>
        position.X >= 0 && position.X < Size &&
        position.Y >= 0 && position.Y < Size;

    private void MakeKing(Checker checker)
    {
        if (
            (checker.Color == Colors[0] && checker.Position.X == Size - 1)
            ||
            (checker.Color == Colors[1] && checker.Position.X == 0)
            )
        {
            checker.IsKing = true;
            OnMakingKing(new MakingKingEventArgs(checker));
        }
    }

    private void OnMakingKing(MakingKingEventArgs makingKingEventArgs)
    {
        MakingKing?.Invoke(null, makingKingEventArgs);
    }

    private void ArrangeCheckers()
    {
        for (int i = 0; i < 3; i++)
            for (int j = i % 2; j < Size; j += 2)
                Checkers.Add(new Checker(new Position(i, j), Colors[0]));

        for (int i = Size - 3; i < Size; i++)
            for (int j = i % 2; j < Size; j += 2)
                Checkers.Add(new Checker(new Position(i, j), Colors[1]));
    }

    private enum Direction
    {
        NextLeft,
        NextRight,
        PreviousLeft,
        PreviousRight,
    }

    private static Direction GetOpositeDirection(Direction direction) => direction switch
    {
        Direction.PreviousRight => Direction.NextLeft,
        Direction.NextLeft => Direction.PreviousRight,
        Direction.NextRight => Direction.PreviousLeft,
        Direction.PreviousLeft => Direction.NextRight,
        _ => throw new ArgumentException()
    };

    private static Dictionary<Direction, Position> Step = new()
    {
        { Direction.NextLeft, new(1, -1) },
        { Direction.NextRight, new(1, 1) },
        { Direction.PreviousLeft, new(-1, -1) },
        { Direction.PreviousRight, new(-1, 1) },
    };

    private IEnumerable<Position> GoByDirection(Position current, Direction direction)
    {
        Position nextPosition = new(current.X + Step[direction].X, current.Y + Step[direction].Y);

        while (IsPositionOnBoard(nextPosition))
        {
            yield return nextPosition;
            nextPosition = new(nextPosition.X + Step[direction].X, nextPosition.Y + Step[direction].Y);
        }
    }

    private void OnRemovingChecker(Checker checker)
    {
        foreach (var attack in Checkers)
            attack.UnderAttackCheckers.Remove(checker);

        RemovingChecker?.Invoke(null, new RemovingCheckerEventArgs(checker.Position));
    }

    private void OnChangingCheckerPosition(Checker checker, Position previous)
    {
        foreach (var attack in Checkers)
            attack.UnderAttackCheckers = new();

        foreach (var temp in Checkers)
            FindAttackingCheckers(temp);

        ChangingCheckerPosition?.Invoke(null, new ChangingCheckerPositionEventArgs(previous, checker.Position, checker.Moves));
    }

    private void FindAttackingCheckers(Checker underAttack)
    {
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            FindAttackingCheckerByDirection(underAttack, direction);
    }

    private void FindAttackingCheckerByDirection(Checker underAttack, Direction direction)
    {
        // Find attacking checker
        int walkDistance = 0;
        Checker? attackChecker = null;

        foreach (var position in GoByDirection(underAttack.Position, direction))
        {
            attackChecker = GetChecker(position);

            if (attackChecker is not null)
                break;

            walkDistance++;
        }


        // Is possible attacking checker?
        if (attackChecker is null)
            return;

        if (attackChecker.Color == underAttack.Color)
            return;

        if (walkDistance > 0 && !attackChecker.IsKing)
            return;


        // Find possible attack position
        List<Position> positionsAfterAttack = new();

        foreach (var position in GoByDirection(underAttack.Position, GetOpositeDirection(direction)))
        {
            if (IsCheckerOnPosition(position))
                break;

            positionsAfterAttack.Add(position);

            if (!attackChecker.IsKing)
                break;
        }

        if (positionsAfterAttack.Count == 0)
            return;

        attackChecker.UnderAttackCheckers.Add(underAttack, positionsAfterAttack);
        attackChecker.Moves.AddRange(positionsAfterAttack);
    }

    public List<Position> FindMovingPositions(Checker checker)
    {
        var result = new List<Position>();
        var directions = Enum.GetValues<Direction>().ToList();

        if (!checker.IsKing)
            if (checker.Color == Colors[0])
            {
                directions.Remove(Direction.PreviousLeft);
                directions.Remove(Direction.PreviousRight);

            }
            else
            {
                directions.Remove(Direction.NextLeft);
                directions.Remove(Direction.NextRight);
            }


        foreach (var direction in directions)
            foreach (var position in GoByDirection(checker.Position, direction))
            {
                if (IsCheckerOnPosition(position))
                    break;

                result.Add(position);

                if (!checker.IsKing)
                    break;
            }

        if (result.Count == 0)
            return new();

        return result;
    }
}