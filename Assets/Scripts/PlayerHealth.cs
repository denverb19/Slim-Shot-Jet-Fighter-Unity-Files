using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float playerHealthMaximum = 5;
    [SerializeField] float delayDesired = 2f;
    [SerializeField] float damageDelayRequired = 0.1f;
    [SerializeField] ParticleSystem playerDeathExplosion;
    private PlayerController playerControllerscript;
    [SerializeField] AudioClip deathSoundClip;
    [SerializeField] int startingMissileCount = 10;
    //[SerializeField] int playerLives = 3;
    public AudioSource playerAudioSource;
    ScoreBoard currentScoreboard;
    HealthSlider healthSliderScript;
    //LifeCounter currentLifeCounter;
    [SerializeField] float playerCurrentHealth = 5;
    private bool takingDamage = false;
    public int upgradeLevel = 0;
    //AudioSource deathSound;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerscript = GetComponent<PlayerController>();
        healthSliderScript = FindObjectOfType<HealthSlider>();
        //currentLifeCounter = FindObjectOfType<LifeCounter>();
        playerCurrentHealth = playerHealthMaximum;
        //deathSound = gameObject.GetComponent<AudioSource>();
        //playerAudioSource = playerControllerscript.playerAudioSource;
        healthSliderScript.UpdateHealth(playerCurrentHealth/playerHealthMaximum);
        StartCoroutine(GetScoreBoard());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GetScoreBoard()
    {
        //playerControllerscript = GetComponent<PlayerController>();
        
        yield return new WaitForSeconds(1f);
        healthSliderScript = FindObjectOfType<HealthSlider>();
        healthSliderScript.UpdateHealth(playerCurrentHealth/playerHealthMaximum);
        currentScoreboard = FindObjectOfType<ScoreBoard>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(!playerControllerscript.isEvading)
        {
            switch (other.gameObject.tag)
            {
                case "Enemy":
                    StartCoroutine(TakeDamage(1f));
                    //CrashSequence();
                    break;
                case "Enemy Weapon":
                    float damageToTake = other.gameObject.GetComponent<MissileControls>().GetDamage();
                    StartCoroutine(TakeDamage(damageToTake));
                    Destroy(other.gameObject, 0f);
                    break;
                case "Terrain":
                    StartCoroutine(TakeDamage(1f));
                    //CrashSequence();
                    break;
                case "Upgrade Pickup":
                    other.gameObject.GetComponent<PickUpScript>().DestroyPickup();
                    UpgradeShip();
                    break;
                default:
                    break;
            }
        }
    }
    public IEnumerator TakeDamage(float damageAmount)
    {
        if(!takingDamage)
        {
            takingDamage = true;
            playerCurrentHealth -= damageAmount;
            healthSliderScript.UpdateHealth(playerCurrentHealth/playerHealthMaximum);
            if(playerCurrentHealth <= 0.0f)
            {
                CrashSequence();
            }
        }
        yield return new WaitForSeconds(damageDelayRequired);
        takingDamage = false;
    }
    void UpgradeShip()
    {
        if(upgradeLevel == 0)
        {
            upgradeLevel = 1;
            GameObject[] upgradeParts = GameObject.FindGameObjectsWithTag("Upgrade 1");
            foreach (GameObject part in upgradeParts)
            {
                part.GetComponent<MeshRenderer>().enabled = true;
                //part.SetActive(true);
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
            /*int currentMissileCount = currentScoreboard.GetMissileCount();
            if(currentMissileCount < startingMissileCount)
            {
                currentScoreboard.ChangeMissileCount(startingMissileCount - currentMissileCount);
            }*/
            //deathSound.Play();
            StartCoroutine(ReloadLevel(delayDesired));
        }
    }
    IEnumerator ReloadLevel(float delayRequired)
    {
        yield return new WaitForSeconds(delayRequired);
        currentScoreboard.decreasePlayerLives(1);
        //currentLifeCounter.UpdatePlayerLives();
        int currentLives = currentScoreboard.getPlayerLives();
        //playerLives -= 1;
        if(currentLives > 0)
        {
            int currentMissileCount = currentScoreboard.GetMissileCount();
            if(currentMissileCount < startingMissileCount)
            {
                currentScoreboard.ChangeMissileCount(startingMissileCount - currentMissileCount);
            }
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            //healthSliderScript.UpdateHealth(1f);
            SceneManager.LoadScene(currentLevel);
        }
        else
        {
            SceneManager.LoadScene(3);//load score screen
        }
    }
}
