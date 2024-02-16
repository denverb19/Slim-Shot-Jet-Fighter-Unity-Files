using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerControllerLevelOne : MonoBehaviour
{
    void Awake()
    {
        MusicPlayerControllerMainMenu mpMainMenu = FindObjectOfType<MusicPlayerControllerMainMenu>();
        if(mpMainMenu != null)
        {
            Destroy(mpMainMenu.gameObject);
        }
        int musicPlayerInstanceCount = FindObjectsOfType<MusicPlayerControllerLevelOne>().Length;
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
