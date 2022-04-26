using Checkers.Core.Events;
using Checkers.Core.Strategies;

namespace Checkers.Core;

public class Player
{
    public event EventHandler<SelectingCheckerEventArgs>? SelectingChecher;
    public event EventHandler<UnselectingCheckerEventArgs>? UnselectingChecher;

    private Checker? selected = null;

    public Checker? Selected
    {
        get => selected;
        set
        {
            if (value is null && selected is not null)
                UnselectingChecher?.Invoke(null, new UnselectingCheckerEventArgs(selected.Position, selected.Moves));


            if(value is not null)
                SelectingChecher?.Invoke(null, new SelectingCheckerEventArgs(value.Position, value.Moves));

            selected = value;
        }
    }

    public Color Color { get; private set; }

    public CheckerBoard Board { get; private set; }

    public IStrategy Strategy { get; internal set; }

    public Player(Color color, CheckerBoard board)
    {
        Color = color;
        Board = board;
        Strategy = new SelectStrategy();
    }

    public void ExecuteStrategy(Position position)
    {
        Strategy.Execute(this, position);
    }
}
