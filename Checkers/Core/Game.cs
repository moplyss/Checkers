using Checkers.Core.Strategies;

namespace Checkers.Core;


public class Game
{
    public event Action? ChangingPlayer;

    public Player[] Players { get; private set; }

    public Player CurrentPlayer { get; private set; }

    public CheckerBoard Board { get; private set; }

    public Game()
    {
        Color color1 = Color.White;
        Color color2 = Color.Black;

        Board = new(color1, color2);

        Players = new Player[2];
        Players[0] = new(color1, Board);
        Players[1] = new(color2, Board);

        CurrentPlayer = Players[0];
    }

    public void SelectPlayerAction(Position position)
    {
        if (CurrentPlayer.Selected is null)
        {
            CurrentPlayer.Strategy = new SelectStrategy();
            CurrentPlayer.ExecuteStrategy(position);
            return;
        }

        if (CurrentPlayer.Selected.Position == position)
        {
            CurrentPlayer.Strategy = new UnselectStrategy();
            CurrentPlayer.ExecuteStrategy(position);
            return;
        }

        if (Board.IsCheckerOnPosition(position))
            throw new Exception();


        if (CurrentPlayer.Selected.UnderAttackCheckers.Count > 0)
        {
            CurrentPlayer.Strategy = new AttackStrategy();
            CurrentPlayer.ExecuteStrategy(position);

            if (CurrentPlayer.Selected.UnderAttackCheckers.Count == 0)
            {
                CurrentPlayer.Selected = null;
                ChangeCurrentPlayer();
            }

            return;
        }

        if (CurrentPlayer.Selected.Moves.Count > 0)
        {
            CurrentPlayer.Strategy = new MoveStrategy();
            CurrentPlayer.ExecuteStrategy(position);
            ChangeCurrentPlayer();
            return;
        }
    }

    private void ChangeCurrentPlayer()
    {
        if (CurrentPlayer == Players[0])
            CurrentPlayer = Players[1];
        else
            CurrentPlayer = Players[0];

        ChangingPlayer?.Invoke();
    }
}
