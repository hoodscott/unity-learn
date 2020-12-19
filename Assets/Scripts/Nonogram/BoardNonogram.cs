using UnityEngine;
using System;

public class BoardNonogram : Board
{
    public GameObject GOClue;
    protected new CellNonogram[,] board;

    private string[] newcluesRow;
    private string[] newcluesCol;

    public override void GenerateBoard()
    {
        base.GenerateBoard();

        PlacePixels();

        CheckClues();
        //PrintClues();
        ShowClues();
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
    }

    private void PlacePixels()
    {
        foreach (CellNonogram cell in board)
            if (rnd.Next(10) > 3)
                cell.SetPixel(true);
    }

    private void PrintClues()
    {
        print("rows");
        foreach (string row in newcluesRow)
        {
            print(row.ToString());
        }
        print("cols");
        foreach (string col in newcluesCol)
        {
            print(col.ToString());
        }
    }

    private void ShowClues()
    {
        float xStart = GetOrigin().x;
        float yStart = GetOrigin().y;
        int y = 0;
        int x;
        string[] r;

        foreach (string row in newcluesRow)
        {
            r = row.Split(' ');
            Array.Reverse(r);
            x = 0;
            foreach (string item in r)
            {
                Vector3 pos = new Vector3(xStart - 1 - x, yStart + y);
                GameObject cell = Instantiate(GOClue, pos, Quaternion.identity);
                cell.transform.SetParent(transform);
                cell.GetComponent<CellClue>().SetClue(int.Parse(item));
                x++;
            }
            y++;
        }

        yStart += y;

        x = 0;

        foreach (string col in newcluesCol)
        {
            r = col.Split(' ');
            y = 0;
            foreach (string item in r)
            {
                Vector3 pos = new Vector3(xStart + x, yStart + y);
                GameObject cell = Instantiate(GOClue, pos, Quaternion.identity);
                cell.transform.SetParent(transform);
                cell.GetComponent<CellClue>().SetClue(int.Parse(item));
                y++;
            }
            x++;
        }
    }

    private void CheckClues()
    {
        newcluesCol = new string[width];
        newcluesRow = new string[height];

        for (int i = 0; i < width; i++)
            newcluesCol[i] = CalcClues(i, true);

        for (int i = 0; i < height; i++)
            newcluesRow[i] = CalcClues(i, false);
    }

    private string CalcClues(int rowCol, bool col)
    {
        int x;
        int y;
        bool isPixel;

        bool prev = false;
        int count = 0;
        string clues = "";

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

            isPixel = board[y, x].IsPixel();

            if (isPixel)
                count++;

            if (i> 0 && prev && !isPixel)
            {
                clues += count.ToString() + " ";
                count = 0;
            }
            prev = isPixel;
        }

        if (count > 0)
            clues += count.ToString() + " ";
        else if (clues.Length == 0)
            clues = "0";

        if (rowCol == 0 && col)
            print(clues);
        return clues.Trim(' ');
    }
}


