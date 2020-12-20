using UnityEngine;
using System.Collections.Generic;

public class BoardNonogram : Board
{
    public GameObject GOClue;
    protected new CellNonogram[,] board;

    private Clues[] cluesRow;
    private Clues[] cluesCol;

    public override void GenerateBoard()
    {
        base.GenerateBoard();

        PlacePixels();

        CreateClues();

        CheckSolved();
    }

    protected override void InitBoard()
    {
        board = new CellNonogram[height, width];
    }

    protected override void AddToBoard(int x, int y, GameObject cell)
    {
        board[y, x] = cell.GetComponent<CellNonogram>();
        board[y, x].SetBoard(this, x, y); //why cant this line be in base class generateboard?
    }

    public override void DblSelectCell(int x, int y, int extra)
    {
        
    }

    public override void FlagCell(int x, int y, bool flag)
    {
        if(gameOver) return;

        board[y, x].SetFlag(flag);
    }

    public override void SelectCell(int x, int y)
    {
        if (gameOver) return;

        board[y, x].Uncover();

        if (board[y, x].IsPixel())
            GameOver("You Lose :(");
        else
            CheckClue(x, y);
    }

    private void PlacePixels()
    {
        foreach (CellNonogram cell in board)
            if (rnd.Next(10) > 3)
                cell.SetPixel(true);
    }

    private void ShowClues(List<int>[] rows, List<int>[] cols)
    {
        cluesRow = new Clues[height];
        cluesCol = new Clues[width];
        float xStart = GetOrigin().x;
        float yStart = GetOrigin().y;
        int x;
        int y = 0;
        int i = 0;

        foreach (List<int> row in rows)
        {
            cluesRow[i] = new Clues();
            row.Reverse();
            x = 0;
            foreach (int clue in row)
            {
                cluesRow[i].Add(CreateClue(xStart - 1 - x, yStart + y, clue));
                if (clue == height)
                    cluesRow[i].SetSolved(true);
                x++;
            }
            y++;
            i++;
        }

        yStart += y;

        x = 0;
        i = 0;

        foreach (List<int> col in cols)
        {
            cluesCol[i] = new Clues();
            y = 0;
            foreach (int clue in col)
            {
                cluesCol[i].Add(CreateClue(xStart + x, yStart + y, clue));
                if (clue == width)
                    cluesCol[i].SetSolved(true);
                y++;
            }
            x++;
            i++;
        }
    }

    private void CreateClues()
    {
        List<int>[] cr = new List<int>[height];
        List<int>[] cc = new List<int>[width];

        for (int i = 0; i < height; i++)
            cr[i] = CalcClues(i, false);

        for (int i = 0; i < width; i++)
            cc[i] = CalcClues(i, true);

        ShowClues(cr, cc);
    }

    private CellClue CreateClue(float x, float y, int val)
    {
        Vector3 pos = new Vector3(x, y);
        GameObject cell = Instantiate(GOClue, pos, Quaternion.identity);
        cell.transform.SetParent(transform);
        CellClue clue = cell.GetComponent<CellClue>();
        clue.SetClue(val);
        return clue;
    }

    private List<int> CalcClues(int rowCol, bool col, bool checkSolve = false)
    {
        int x;
        int y;
        bool isPixel;

        bool prev = false;
        int count = 0;
        List<int> clues = new List<int>();

        int max = col ? height : width;

        for (int i = 0; i < max; i++)
        {
            if (col)
            {
                y = i;
                x = rowCol;
            } else
            {
                x = i;
                y = rowCol; 
            }

            isPixel = checkSolve ? board[y, x].IsCovered() : board[y, x].IsPixel();

            if (isPixel)
                count++;

            if (i> 0 && prev && !isPixel)
            {
                clues.Add(count);
                count = 0;
            }
            prev = isPixel;
        }

        if (count > 0)
            clues.Add(count);
        else if (clues.Count == 0) 
            clues.Add(0);

        return clues;
    }

    private void CheckClue(int x, int y)
    {        
        cluesCol[x].CompareSolution(CalcClues(x, true, true));
        cluesRow[y].CompareSolution(CalcClues(y, false, true), true);

        CheckSolved();
    }

    private void CheckSolved()
    {
        foreach (Clues clue in cluesCol)
            if (!clue.GetSolved()) return;

        foreach (Clues clue in cluesRow) 
            if (!clue.GetSolved()) return;

        GameOver("You Win!");
    }
}

public class Clues
{
    private bool solved = false;
    public List<CellClue> clues = new List<CellClue>();

    public void Add(CellClue clue)
    {
        clues.Add(clue);
    }

    public void CompareSolution(List<int> solution, bool row = false)
    {
        if (row) solution.Reverse();

        if (solution.Count != clues.Count)
        {
            SetSolved(false);
            return;
        }

        for (int i = 0; i < solution.Count; i++)
        {
            if (solution[i] != clues[i].GetClue())
            {
                SetSolved(false);
                return;
            }
        }
        SetSolved(true);
    }

    public bool GetSolved()
    {
        return solved;
    }

    public void SetSolved(bool solved)
    {
        this.solved = solved;
        //todo = figure out how to only shade "solved" clues instead of waiting on whole row
        foreach (CellClue clue in clues)
            clue.SetSolved(solved);
    }
}
