using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Microsoft.Unity.VisualStudio.Editor;
//using Palmmedia.ReportGenerator.Core.Parser.Analysis;

public class ScoreBoard : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentScore = 0;
    [SerializeField] int playerLives = 3;
    [SerializeField] TMP_Text livesCountText;
    [SerializeField] Image engineImage;
    TMP_Text scoreText;
    void Awake()
    {
        int scoreBoardInstanceCount = FindObjectsOfType<ScoreBoard>().Length;
        if ( scoreBoardInstanceCount > 1)
        {
            //Destroy(gameObject.transform.parent.gameObject);
            Destroy(gameObject.transform.parent.transform.parent.gameObject);
        }
        else
        {
            //DontDestroyOnLoad(gameObject.transform.parent.gameObject);
            DontDestroyOnLoad(gameObject.transform.parent.transform.parent.gameObject);
        }
    }
    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        livesCountText.text = playerLives.ToString("0") + "\nLives";
        //scoreText.text = "Test Passed";
    }
    public void IncreaseScore(int incrementPassed)
    {
        currentScore += incrementPassed;
        //scoreText.text = " Score: " + currentScore.ToString("000000");
        scoreText.text = currentScore.ToString("000000");
        //Debug.Log("Current Score: " + currentScore);
    }
    public void decreasePlayerLives(int livesLost)
    {
        playerLives -= livesLost;
        livesCountText.text = playerLives.ToString("0") + "\nLives";
    }
    public int getPlayerLives()
    {
        return playerLives;
    }
    public int GetScore()
    {
        return currentScore;
    }
    public void ChangeEngineColor(int redColor, int greenColor, int blueColor, int opacityLevel)
    {
        engineImage.GetComponent<Image>().color = new Color32((byte)redColor, (byte)greenColor, (byte)blueColor, (byte)opacityLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
