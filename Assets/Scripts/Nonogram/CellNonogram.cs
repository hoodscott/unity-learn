using UnityEngine;

public class CellNonogram : Cell
{
    public Sprite[] texturesUncovered;

    private bool flag;
    private bool pixel;

    public void OnMouseOver()
    {
        if (covered && Input.GetMouseButtonUp(1))
        {
            board.FlagCell(x, y, !flag);
        }
    }

    public void SetPixel(bool isPixel)
    {
        pixel = isPixel;
    }

    public bool IsPixel()
    {
        return pixel;
    }

    public void Uncover()
    {
        if (!flag)
        {
            GetComponent<SpriteRenderer>().sprite = texturesUncovered[pixel ? 1 : 0];
            covered = false;
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

}
