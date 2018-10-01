using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] private int lifes = 3;
    [SerializeField] private moveBall ball;
    [SerializeField] private Text scoreTxt;
    private int points;
    private int blocks = 0;
    [SerializeField] private GameObject[] levels;
    private int currLvl;
    private GameObject currBoard;
    private int bestScore = 0;
    private bool gameOver = false;
    public GameObject btnRetry, btnQuit, btnContinue;

    public void Init()
    {
        points = 0;
        lifes = 3;
        bestScore = PlayerPrefs.GetInt("Score", 0);
        LoadLvl();
        scoreTxt.text = "SCORE : " + points.ToString("D8");

        if (gameOver == true)
        {
            ball.gameObject.SetActive(true);
            ball.Init();
            gameOver = false;
        }

        btnRetry.SetActive(false);
        btnContinue.SetActive(false);
        btnQuit.SetActive(false);
    }

    public void Restart() // recommencer depuis le début
    {
        currLvl = 0;
        Init();
    }

    public void Continue() // continuer au niveau actuel
    {
        Init();
    }

    public void LoadLvl()
    {
        if (currBoard)
        {
            Destroy(currBoard);
        }
        blocks = 0;
        currBoard = Instantiate(levels[currLvl]);
    }

    public void AddBlock()
    {
        blocks++;
    }

    public void Death()
    {
        lifes--;
        if (lifes > 0)
        {
            ball.Init();
        }
        else
        {
            scoreTxt.text = "GAME OVER\nSCORE : " + points.ToString("D8");
            ball.gameObject.SetActive(false);
            btnQuit.gameObject.SetActive(true);
            btnRetry.gameObject.SetActive(true);
            btnContinue.gameObject.SetActive(true);
            gameOver = true;
            if (points > bestScore)
            {
                PlayerPrefs.SetInt("Score", points);
                PlayerPrefs.Save();
            }
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void AddPoints()
    {
        points += 1000;
        string preText = "";
        if (points > bestScore)
        {
            preText = "BEST ";
        }
        scoreTxt.text = preText + "SCORE : " + points.ToString("D8");
        blocks--;
        if (blocks <= 0)
        {
            if (currLvl < levels.Length - 1)
            {
                currLvl++;
            }
            else
            {
                Debug.Log("Replay last level");
            }
            ball.Init();
            LoadLvl();
        }
    }

    // Use this for initialization
    void Start () {
        Restart();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
