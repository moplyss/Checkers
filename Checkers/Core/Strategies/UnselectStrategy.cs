namespace Checkers.Core.Strategies;

public class UnselectStrategy : IStrategy
{
    public void Execute(Player player, Position position)
    {
        if (player.Selected is null)
            throw new Exception();

        if (player.Selected.Position != position)
            throw new Exception();

        player.Selected = null;
    }
}
