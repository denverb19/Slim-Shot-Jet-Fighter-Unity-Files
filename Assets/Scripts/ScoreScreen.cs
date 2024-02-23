using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text finalScoreText;
    void Start()
    {
        ScoreBoard currentBoard = FindObjectOfType<ScoreBoard>();
        finalScoreText.text = "Congratulations\nYour Final Score Was\n" + currentBoard.GetScore().ToString("000000");
        Destroy(currentBoard.gameObject.transform.parent.transform.parent.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
