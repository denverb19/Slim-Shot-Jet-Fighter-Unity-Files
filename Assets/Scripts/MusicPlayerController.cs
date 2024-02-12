using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        int musicPlayerInstanceCount = FindObjectsOfType<MusicPlayerController>().Length;
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
