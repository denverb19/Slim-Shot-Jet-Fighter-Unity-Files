using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;

public class ScoreBoard : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentScore = 0;
    TMP_Text scoreText;

    public void IncreaseScore(int incrementPassed)
    {
        currentScore += incrementPassed;
        scoreText.text = " Score: " + currentScore.ToString("000000");
        //Debug.Log("Current Score: " + currentScore);
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
