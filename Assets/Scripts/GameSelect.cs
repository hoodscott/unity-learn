using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelect : MonoBehaviour
{
    /**
     * simple games made to learn different 2D puzzle game techniques in Unity
     * 
     * improvements:
     * background music that persists after level select
     * learn how to use the UI elements (bit sloppy atm)
     * 
     * new games:
     * nonogram game (random puzzles at first)
     * tetris
     * nonogram with level select
     * 
     */

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void StartMinesweeper()
    {
        SceneManager.LoadScene(1);
    }

    public void StartNonogram()
    {
        SceneManager.LoadScene(2);
    }
}
