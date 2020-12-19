using UnityEngine;

public class CellMinesweeper : Cell
{
    public Sprite[] texturesEmpty;
    public Sprite textureMine;

    private bool flag;
    private bool mine;
    private int adjacent;

    private int clicked;
    private float clickTime;
    private readonly float clickDelay = 1.5f;

    public void OnMouseOver()
    {
        if (covered && Input.GetMouseButtonUp(1))
        {
            board.FlagCell(x, y, !flag);
        } else if (!covered && !mine && Input.GetMouseButtonUp(0))
        {
            clicked++;
            if (clicked == 1) clickTime = Time.time;

            if (clicked > 1 && Time.time - clickTime < clickDelay)
            {
                clicked = 0;
                clickTime = 0;
                board.DblSelectCell(x, y, adjacent);
            }
            else if (clicked > 2 || Time.time - clickTime > 1)
            {
                clicked = 0;
            }
        }
    }

    public void SetMine(bool isMine)
    {
        mine = isMine;
    }

    public bool IsMine()
    {
        return mine;
    }

    public void Uncover(int adjacent)
    {
        if (!flag)
        {
            GetComponent<SpriteRenderer>().sprite = mine ? textureMine : texturesEmpty[adjacent];
            covered = false;
            this.adjacent = adjacent;
        }
    }

    public void SetFlag(bool flag)
    {
        this.flag = flag;
        GetComponent<SpriteRenderer>().sprite = flag ? textureFlag : textureDefault;
    }

    public bool IsFlag()
    {
        return flag;
    }

    public int GetAdjacent()
    {
        return adjacent;
    }
}
