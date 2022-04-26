using Checkers.Core;
using Checkers.Core.Events;
using Checkers.Core.GameEventArgs;

namespace Checkers;

public partial class Window : Form
{
    private const int gridCellSize = 50;

    private readonly Image whiteChekerImage = new Bitmap(new Bitmap(@"..\..\..\assets\whiteCheckerImage.png"), new Size(gridCellSize - 10, gridCellSize - 10));
    private readonly Image blackChekerImage = new Bitmap(new Bitmap(@"..\..\..\assets\blackCheckerImage.png"), new Size(gridCellSize - 10, gridCellSize - 10));

    private readonly Dictionary<Position, Button> grid = new();

    private Game game = new();

    public Window()
    {
        InitializeComponent();
        Init();
    }

    public void Init()
    {
        DrawBoard();

        foreach (Player player in game.Players)
        {
            player.SelectingChecher += UpdateAfterSelectingChecker;
            player.UnselectingChecher += UpdateAfterUnselectingChecker;
        }

        game.ChangingPlayer += UpdateAfterChangingPlayer;
        game.Board.MakingKing += UpdateAfterMakingKinkg;
        game.Board.RemovingChecker += UpdateAfterRemovingChecker;
        game.Board.ChangingCheckerPosition += UpdateAfterMovingChecker;
    }

    private void DrawBoard()
    {
        for (int x = 0; x < CheckerBoard.Size; x++)
            for (int y = 0; y < CheckerBoard.Size; y++)
            {
                Button gridCell = new()
                {
                    Size = new Size(gridCellSize, gridCellSize),
                    Font = new Font("Arial", 22, FontStyle.Bold),
                    Location = new Point(x * gridCellSize, y * gridCellSize),
                    BackColor = (x % 2 == 1 && y % 2 == 0) || (x % 2 == 0 && y % 2 == 1) ? Color.White : Color.Gray,
                    ForeColor = Color.Red
                };

                if (gridCell.BackColor == Color.Gray)
                {
                    Position gridCellPosition = new(x, y);
                    gridCell.Click += (_, _) => game.SelectPlayerAction(gridCellPosition);
                    grid.Add(gridCellPosition, gridCell);
                }
                else
                {
                    gridCell.Enabled = false;
                }

                Controls.Add(gridCell);
            }

        foreach (var pair in game.Board.Checkers)
            DrawChecker(pair);
    }

    private void DrawChecker(Checker checker)
    {
        Button cell = grid[checker.Position];
        cell.Image = (checker.Color == Color.Black) ? blackChekerImage : whiteChekerImage;
    }

    private void UpdateAfterMovingChecker(object? _, ChangingCheckerPositionEventArgs args)
    {
        grid[args.Next].Text = grid[args.Previous].Text;
        grid[args.Next].Image = grid[args.Previous].Image;

        grid[args.Previous].Text = "";
        grid[args.Previous].Image = null;

        if (args.Moves.Count > 0)
        {
            foreach (var gridCell in grid)
            {
                gridCell.Value.Enabled = false;
                gridCell.Value.BackColor = Color.Gray;
            }

            foreach (var position in args.Moves)
            {
                grid[position].Enabled = true;
                grid[position].BackColor = Color.Green;
            }
        }

    }


    private void UpdateAfterMakingKinkg(object? _, MakingKingEventArgs args)
    {
        grid[args.Position].Text = "D";
    }

    private void UpdateAfterRemovingChecker(object? _, RemovingCheckerEventArgs args)
    {
        grid[args.Position].Image = null;
    }

    private void UpdateAfterSelectingChecker(object? _, SelectingCheckerEventArgs args)
    {
        foreach (var gridCell in grid)
            gridCell.Value.Enabled = false;

        grid[args.Position].Enabled = true;

        foreach (var position in args.Moves)
        {
            grid[position].Enabled = true;
            grid[position].BackColor = Color.Green;
        }
    }

    private void UpdateAfterUnselectingChecker(object? _, UnselectingCheckerEventArgs args)
    {
        foreach (var gridCell in grid)
            gridCell.Value.Enabled = true;

        foreach (var position in args.Moves)
            grid[position].BackColor = Color.Gray;
    }

    private void UpdateAfterChangingPlayer()
    {
        foreach (var gridCell in grid)
        {
            if (gridCell.Value.BackColor == Color.Green)
                gridCell.Value.BackColor = Color.Gray;

            gridCell.Value.Enabled = true;
        }

    }
}
