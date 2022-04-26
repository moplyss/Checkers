namespace Checkers.Core.Strategies;

public class AttackStrategy : IStrategy
{
    public void Execute(Player player, Position position)
    {
        if (player.Selected is null)
            throw new Exception();

        if (player.Board.IsCheckerOnPosition(position))
            throw new Exception();

        foreach (var pair in player.Selected.UnderAttackCheckers)
            if (pair.Value.Contains(position))
            {
                player.Board.RemoveChecker(pair.Key);
                player.Board.ChangeCheckerPosition(player.Selected, position);

                return;
            }

        throw new Exception();
    }
}
