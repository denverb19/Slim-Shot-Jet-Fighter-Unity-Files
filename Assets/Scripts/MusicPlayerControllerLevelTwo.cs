using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerControllerLevelTwo : MonoBehaviour
{
    void Awake()
    {
        MusicPlayerControllerMainMenu mpMainMenu = FindObjectOfType<MusicPlayerControllerMainMenu>();
        if(mpMainMenu != null)
        {
            Destroy(mpMainMenu.gameObject);
        }
        MusicPlayerControllerLevelOne mpLevelOne = FindObjectOfType<MusicPlayerControllerLevelOne>();
        if(mpLevelOne != null)
        {
            Destroy(mpLevelOne.gameObject);
        }
        int musicPlayerInstanceCount = FindObjectsOfType<MusicPlayerControllerLevelTwo>().Length;
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
