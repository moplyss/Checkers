namespace Checkers.Core.Strategies;

public class MoveStrategy : IStrategy
{
    public void Execute(Player player, Position position)
    {
        if (player.Selected is null)
            throw new Exception();

        if (player.Selected.Moves.Count == 0)
            throw new Exception();

        player.Board.ChangeCheckerPosition(player.Selected, position);
        player.Selected = null;
    }
}
