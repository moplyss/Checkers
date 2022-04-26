namespace Checkers.Core.Strategies;

public interface IStrategy
{
    public void Execute(Player player, Position position);
}
