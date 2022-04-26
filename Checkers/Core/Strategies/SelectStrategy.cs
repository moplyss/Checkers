namespace Checkers.Core.Strategies;

public class SelectStrategy : IStrategy
{
    public void Execute(Player player, Position position)
    {
        var checker = player.Board.GetChecker(position);
        if (checker == null || checker.Color != player.Color)
            throw new Exception();

        var attacking = player.Board.Checkers.FindAll(c => c.Color == player.Color && c.UnderAttackCheckers.Count > 0);

        if(attacking.Count > 0 && !attacking.Contains(checker))
            throw new Exception();

        if (checker.UnderAttackCheckers.Count > 0)
            checker.Moves = checker.UnderAttackCheckers.SelectMany(d => d.Value).ToList();
        else
            checker.Moves = player.Board.FindMovingPositions(checker);


        player.Selected = checker;
    }
}
