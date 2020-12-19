using UnityEngine;

public class CellClue : Cell
{
    public Sprite[] texturesClues;

    private int clue;
    private bool solved;

    public void SetClue(int clue)
    {
        this.clue = clue;
        UpdateSprite();
    }

    public void SetSolved(bool solved)
    {
        this.solved = solved;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (!solved)
            GetComponent<SpriteRenderer>().sprite = texturesClues[this.clue];
    }
}
