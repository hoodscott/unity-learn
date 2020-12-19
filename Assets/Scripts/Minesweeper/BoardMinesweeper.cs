using System;
using TMPro;
using UnityEngine;

public class BoardMinesweeper : Board
{
    public TextMeshProUGUI tmpMinesRemaining;

    protected new CellMinesweeper[,] board;

    private int mines;
    private int remainingMines;
    private bool newGame;
    private float timeStart;

    /** created using tutorial https://noobtuts.com/unity/2d-minesweeper-game
     * 
     * improvements:
     * scoreboards (persistent after close/reopen ie. saves)
     * autosolve (could also be used to precheck puzzle is solvable without guesswork)
     * seeds (using the noise thing rather than random)
     * redo sprites (not windows 98 themed)
     * 
     */

    public override void GenerateBoard()
    {
        base.GenerateBoard();

        remainingMines = mines;
        UpdateMineUI();
        newGame = true;
        timeStart = Time.time;
    }

    protected override void InitBoard()
    {
        board = new CellMinesweeper[height, width];
    }

    protected override void AddToBoard(int x, int y, GameObject cell)
    {
        board[y, x] = cell.GetComponent<CellMinesweeper>();
        board[y, x].SetBoard(this, x, y); //why cant this line be in base class generateboard?
    }

    protected override void SelectDifficulty()
    {
        switch (tmpDifficulty.value)
        {
            case 0:
                height = 10;
                width = 10;
                mines = 10;
                break;
            case 1:
                height = 16;
                width = 16;
                mines = 40;
                break;
            default:
                height = 16;
                width = 30;
                mines = 99;
                break;
        }
    }

    public override void SelectCell(int x, int y)
    {
        if (gameOver) return;

        if (newGame)
        {
            PlaceMines(x, y);
            newGame = false;
        }

        if (IsMine(x, y) == 1)
        {
            if (IsFlag(x, y) == 0)
                UncoverMines();
        }
        else
            UncoverClues(x, y);
    }

    public override void FlagCell(int x, int y, bool flag)
    {
        if (gameOver) return;
        
        board[y, x].SetFlag(flag);
        remainingMines += flag ? -1 : 1;
        UpdateMineUI();
    }
    public override void DblSelectCell(int x, int y, int adjacent)
    {
        if (adjacent == AdjacentFlags(x, y))
        {
            SelectCell(x - 1, y - 1);
            SelectCell(x - 1, y);
            SelectCell(x - 1, y + 1);
            SelectCell(x, y - 1);
            SelectCell(x, y + 1);
            SelectCell(x + 1, y - 1);
            SelectCell(x + 1, y);
            SelectCell(x + 1, y + 1);
        }
    }

    private void PlaceMines(int xClick, int yClick)
    {
        int minesPlaced = 0;
        while (minesPlaced < mines)
        {
            int x = rnd.Next(width);
            int y = rnd.Next(height);
            if (!(x == xClick && y == yClick) && !board[y, x].IsMine())
            {
                board[y, x].SetMine(true);
                minesPlaced++;
            }
        }
    }

    private void UncoverClues(int x, int y)
    {
        if (IsInBounds(x, y))
        {
            board[y, x].Uncover(AdjacentMines(x, y));
            if (board[y, x].GetAdjacent() == 0 && !board[y, x].IsMine())
                UncoverEmpty(x, y, new bool[height, width]);
            CheckWin();
        }
    }

    public void UncoverMines()
    {
        foreach (CellMinesweeper cell in board)
            if (cell.IsMine())
                cell.Uncover(0);

        GameOver("You Lost");
    }

    public void UncoverEmpty(int x, int y, bool[,] visited)
    {
        if (IsInBounds(x, y))
        {
            if (visited[y, x]) return;

            int adjacent = AdjacentMines(x, y);
            board[y, x].Uncover(adjacent);

            if (adjacent == 0)
            {
                visited[y, x] = true;
                UncoverEmpty(x - 1, y - 1, visited);
                UncoverEmpty(x - 1, y, visited);
                UncoverEmpty(x - 1, y + 1, visited);
                UncoverEmpty(x, y - 1, visited);
                UncoverEmpty(x, y + 1, visited);
                UncoverEmpty(x + 1, y - 1, visited);
                UncoverEmpty(x + 1, y, visited);
                UncoverEmpty(x + 1, y + 1, visited);
            }
        }
    }

    public int AdjacentMines(int x, int y)
    {
        return
           IsMine(x - 1, y - 1) + IsMine(x, y - 1) + IsMine(x + 1, y - 1) +
           IsMine(x - 1, y) + IsMine(x + 1, y) +
           IsMine(x - 1, y + 1) + IsMine(x, y + 1) + IsMine(x + 1, y + 1);
    }

    public int IsMine(int x, int y)
    {
        return IsInBounds(x, y) && board[y, x].IsMine() ? 1 : 0;
    }

    public int AdjacentFlags(int x, int y)
    {
        return
           IsFlag(x - 1, y - 1) + IsFlag(x, y - 1) + IsFlag(x + 1, y - 1) +
           IsFlag(x - 1, y) + IsFlag(x + 1, y) +
           IsFlag(x - 1, y + 1) + IsFlag(x, y + 1) + IsFlag(x + 1, y + 1);
    }

    public int IsFlag(int x, int y)
    {
        return IsInBounds(x, y) && board[y, x].IsFlag() ? 1 : 0;
    }

    public void CheckWin()
    {
        int remainingCovered = 0;

        foreach (Cell cell in board)
            if (cell.IsCovered())
                remainingCovered++;


        if (remainingCovered == mines)
            GameOver($"You Won in { Math.Round(Time.time - timeStart, 2) }s");
    }

    private void UpdateMineUI()
    {
        tmpMinesRemaining.SetText($"Remaining Mines: { remainingMines }");
    }
}
