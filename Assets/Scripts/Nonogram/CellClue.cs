using UnityEngine;

public class CellClue : Cell
{
    public Sprite[] texturesClues;

    private int clue;
    private bool solved;

    public int GetClue()
    {
        return clue;
    }

    public void SetClue(int clue)
    {
        covered = false;
        this.clue = clue;
        SetSprite();
    }

    public void SetSolved(bool solved)
    {
        this.solved = solved;
        UpdateSprite();
    }

    private void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = texturesClues[this.clue];
    }

    private void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().color = solved ? new Color(0.5f, 0.5f, 0.5f) : new Color(1, 1, 1);
    }

}
