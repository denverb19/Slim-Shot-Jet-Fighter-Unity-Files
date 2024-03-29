using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerControllerMainMenu : MonoBehaviour
{
    void Awake()
    {
        MusicPlayerControllerLevelOne mpLevelOne = FindObjectOfType<MusicPlayerControllerLevelOne>();
        if(mpLevelOne != null)
        {
            Destroy(mpLevelOne.gameObject);
        }
        MusicPlayerControllerLevelTwo mpLevelTwo = FindObjectOfType<MusicPlayerControllerLevelTwo>();
        if(mpLevelTwo != null)
        {
            Destroy(mpLevelTwo.gameObject);
        }
        ScoreBoard currentBoard = FindObjectOfType<ScoreBoard>();
        if(currentBoard != null)
        {
            //Destroy(currentBoard.gameObject.transform.parent.gameObject);
            Destroy(currentBoard.gameObject.transform.parent.transform.parent.gameObject);
        }
        int musicPlayerInstanceCount = FindObjectsOfType<MusicPlayerControllerMainMenu>().Length;
        if ( musicPlayerInstanceCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
