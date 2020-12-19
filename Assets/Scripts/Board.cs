using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Board : MonoBehaviour
{
    public GameObject GOCell;
    public TextMeshProUGUI tmpGameOver;
    public TMP_Dropdown tmpDifficulty;

    protected Cell[,] board;
    protected int width = 10;
    protected int height = 10;
    protected Vector2 origin;

    protected bool gameOver;

    protected readonly System.Random rnd = new System.Random();

    public virtual void GenerateBoard()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        SelectDifficulty();

        InitBoard();

        float xStart = (width / 2 * -1);
        float yStart = (height / 2 * -1);
        if (width % 2 == 0)
            xStart += 0.5f;
        if (height % 2 == 0)
            yStart += 0.5f;
        SetOrigin(xStart, yStart);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(xStart + x, yStart + y);
                GameObject cell = Instantiate(GOCell, pos, Quaternion.identity);
                cell.transform.SetParent(transform);
                AddToBoard(x, y, cell);
            }

        gameOver = false;
        tmpGameOver.SetText("");
    }

    protected virtual void InitBoard()
    {
        board = new Cell[height, width];
    }

    protected virtual void AddToBoard(int x, int y, GameObject cell)
    {
        board[y, x] = cell.GetComponent<Cell>();
        board[y, x].SetBoard(this, x, y);
    }

    protected virtual void SelectDifficulty()
    {
        switch (tmpDifficulty.value)
        {
            case 0:
                height = 5;
                width = 5;
                break;
            case 1:
                height = 10;
                width = 10;
                break;
            default:
                height = 15;
                width = 15;
                break;
        }
    }

    private void SetOrigin(float x, float y)
    {
        origin = new Vector2(x, y);
    }

    protected Vector2 GetOrigin()
    {
        return origin;
    }

    public abstract void SelectCell(int x, int y);

    public abstract void FlagCell(int x, int y, bool marked);

    public abstract void DblSelectCell(int x, int y, int extra);

    protected void GameOver(string msg)
    {
        gameOver = true;
        tmpGameOver.SetText(msg);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    protected bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
}


