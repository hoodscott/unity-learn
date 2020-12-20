using UnityEngine;

public class Cell : MonoBehaviour
{
    public Sprite textureDefault;
    public Sprite textureFlag;
    protected Board board;

    protected int x;
    protected int y;
    protected bool covered = true;

    public virtual void OnMouseUpAsButton()
    {
        if (IsCovered())
            Uncover();
    }

    public bool IsCovered()
    {
        return covered;
    }

    private void Uncover()
    {
        board.SelectCell(x, y);
    }

    public void SetBoard(Board b, int x, int y)
    {
        board = b;
        this.x = x;
        this.y = y;
    }
}
