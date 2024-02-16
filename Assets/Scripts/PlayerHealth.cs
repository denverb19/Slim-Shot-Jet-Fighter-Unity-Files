using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float delayDesired = 2f;
    [SerializeField] ParticleSystem playerDeathExplosion;
    private PlayerController playerControllerscript;
    [SerializeField] AudioClip deathSoundClip;
    //[SerializeField] int playerLives = 3;
    public AudioSource playerAudioSource;
    ScoreBoard currentScoreboard;
    //AudioSource deathSound;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerscript = GetComponent<PlayerController>();
        //deathSound = gameObject.GetComponent<AudioSource>();
        //playerAudioSource = playerControllerscript.playerAudioSource;
        StartCoroutine(getScoreBoard());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator getScoreBoard()
    {
        yield return new WaitForSeconds(1);
        currentScoreboard = FindObjectOfType<ScoreBoard>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(!playerControllerscript.isEvading)
        {
            switch (other.gameObject.tag)
            {
                case "Enemy":
                    CrashSequence();
                    break;
                case "Terrain":
                    CrashSequence();
                    break;
                default:
                    break;
            }
        }
    }
    void CrashSequence()
    {
        if (playerControllerscript.enabled == true)
        {
            playerControllerscript.enabled = false;
            Renderer[] shipPieces = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in shipPieces)
            {
                r.enabled = false;
            }
            playerDeathExplosion.GetComponent<Renderer>().enabled = true;
            playerDeathExplosion.Play();
            playerAudioSource.clip = deathSoundClip;
            playerAudioSource.Play();
            //deathSound.Play();
            StartCoroutine(ReloadLevel(delayDesired));
        }
    }
    IEnumerator ReloadLevel(float delayRequired)
    {
        yield return new WaitForSeconds(delayRequired);
        currentScoreboard.decreasePlayerLives(1);
        int currentLives = currentScoreboard.getPlayerLives();
        //playerLives -= 1;
        if(currentLives > 0)
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevel);
        }
        else
        {
            SceneManager.LoadScene(0);//load main menu
        }
    }
}
