using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    [SerializeField] Vector2 moveSpeed;
    //Vector2 thisOffset;
    Material thisMaterial;
    void Start()
    {
        thisMaterial = GetComponent<SpriteRenderer>().material;
        /*ScoreBoard currentBoard = FindObjectOfType<ScoreBoard>();
        if(currentBoard != null)
        {
            Destroy(currentBoard.gameObject.transform.parent.gameObject);
        }*/
        /*MusicPlayerControllerLevelOne mpLevelOne = FindObjectOfType<MusicPlayerControllerLevelOne>();
        if(mpLevelOne != null)
        {
            Destroy(mpLevelOne.gameObject);
        }*/
        //StartCoroutine(clearScoreBoard());
    }

    void Update()
    {
        Vector2 deltaOffset = moveSpeed * Time.deltaTime;
        thisMaterial.mainTextureOffset += deltaOffset;
    }
    /*IEnumerator clearScoreBoard()
    {
        yield return new WaitForSeconds(0.01f);
        ScoreBoard currentBoard = FindObjectOfType<ScoreBoard>();
        if(currentBoard != null)
        {
            Destroy(currentBoard.gameObject.transform.parent.gameObject);
        }
        //Destroy(FindObjectOfType<ScoreBoard>().gameObject.transform.parent.gameObject);
    }*/
}