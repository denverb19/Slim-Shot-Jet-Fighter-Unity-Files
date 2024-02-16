using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using Palmmedia.ReportGenerator.Core.Parser.Analysis;

public class ScoreBoard : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentScore = 0;
    [SerializeField] int playerLives = 3;
    TMP_Text scoreText;
    void Awake()
    {
        int scoreBoardInstanceCount = FindObjectsOfType<ScoreBoard>().Length;
        if ( scoreBoardInstanceCount > 1)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        }
    }
    public void IncreaseScore(int incrementPassed)
    {
        currentScore += incrementPassed;
        scoreText.text = " Score: " + currentScore.ToString("000000");
        //Debug.Log("Current Score: " + currentScore);
    }
    public void decreasePlayerLives(int livesLost)
    {
        playerLives -= livesLost;
    }
    public int getPlayerLives()
    {
        return playerLives;
    }
    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        //scoreText.text = "Test Passed";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
