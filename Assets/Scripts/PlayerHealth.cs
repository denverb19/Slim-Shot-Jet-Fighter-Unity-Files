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
    [SerializeField] float upgradeCooldownTime = 0.2f;
    [SerializeField] float landingTimeNeeded = 2f;
    private bool takingDamage = false;
    private bool isUpgradeOnCooldown = false;
    public int upgradeLevel = 0;
    public bool isLanding = false;
    float xLandingVelocity;
    float xLandingVelocityDelta;
    float yLandingVelocity;
    float yLandingVelocityDelta;
    /*float xLandingRotationVelocity;
    float xLandingRotationVelocityDelta;
    float yLandingRotationVelocity;
    float yLandingRotationVelocityDelta;
    float zLandingRotationVelocity;
    float zLandingRotationVelocityDelta;*/
    //float zLandingVelocity;
    //float zLandingVelocityDelta;
    Vector3 preLandingPosition;
    //Vector3 preLandingRotation;
    //Quaternion preLandingRotation;
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
        if(isLanding)
        {
            ProcessLandingMovement();
            //ProcessLandingRotation();
        }
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
        switch (other.gameObject.tag)
        {
            case "Enemy":
                if(!playerControllerscript.isEvading)
                {
                    StartCoroutine(TakeDamage(1f));
                }
                //StartCoroutine(TakeDamage(1f));
                //CrashSequence();
                break;
            case "Enemy Weapon":
                if(!playerControllerscript.isEvading)
                {
                    float damageToTake = other.gameObject.GetComponent<MissileControls>().GetDamage();
                    StartCoroutine(TakeDamage(damageToTake));
                    Destroy(other.gameObject, 0f);
                }
                break;
            case "Terrain":
                if(!playerControllerscript.isEvading)
                {
                    StartCoroutine(TakeDamage(1f));
                }
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
            isUpgradeOnCooldown = true;
            upgradeLevel = 1;
            GameObject[] upgradeParts = GameObject.FindGameObjectsWithTag("Upgrade 1");
            foreach (GameObject part in upgradeParts)
            {
                part.GetComponent<MeshRenderer>().enabled = true;
                //part.SetActive(true);
            }
            playerControllerscript.SetUpgradeLevel(1);
            StartCoroutine(ToggleUpgradeCooldown(upgradeCooldownTime));
        }
        else if(upgradeLevel == 1 && !isUpgradeOnCooldown)
        {
            isUpgradeOnCooldown = true;
            upgradeLevel = 2;
            GameObject[] upgradeParts = GameObject.FindGameObjectsWithTag("Upgrade 2");
            foreach (GameObject part in upgradeParts)
            {
                part.GetComponent<MeshRenderer>().enabled = true;
                //part.SetActive(true);
            }
            playerControllerscript.SetUpgradeLevel(2);
            StartCoroutine(ToggleUpgradeCooldown(upgradeCooldownTime));
        }
    }
    void CrashSequence()
    {
        if (playerControllerscript.enabled == true)
        {
            playerControllerscript.enabled = false;
            //DisableShipMovement();
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
            SceneManager.LoadScene(4);//load score screen
        }
    }
    IEnumerator ToggleUpgradeCooldown(float thisUpgradeCooldownTime)
    {
        yield return new WaitForSeconds(thisUpgradeCooldownTime);
        isUpgradeOnCooldown = false;
    }
    /*public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }*/
    void ProcessLandingMovement()
    {
        xLandingVelocityDelta = xLandingVelocity * Time.deltaTime;
        yLandingVelocityDelta = yLandingVelocity * Time.deltaTime;
        transform.localPosition = new Vector3(
            transform.localPosition.x + xLandingVelocityDelta,
            transform.localPosition.y + yLandingVelocityDelta,
            transform.localPosition.z);
    }
    /*void ProcessLandingRotation()
    {
        xLandingRotationVelocityDelta = xLandingRotationVelocity * Time.deltaTime;
        yLandingRotationVelocityDelta = yLandingRotationVelocity * Time.deltaTime;
        zLandingRotationVelocityDelta = zLandingRotationVelocity * Time.deltaTime;
        transform.localRotation = new Quaternion(
            transform.localRotation.x + xLandingRotationVelocityDelta,
            transform.localRotation.y + yLandingRotationVelocityDelta,
            transform.localRotation.z +  zLandingRotationVelocityDelta, 1f);
        /*transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles.x - xLandingRotationVelocityDelta,
            transform.localRotation.eulerAngles.y + yLandingRotationVelocityDelta,
            transform.localRotation.eulerAngles.z -  zLandingRotationVelocityDelta);
    }*/
    IEnumerator ProcessLandingRotation(float landingTime)
    {
        float t = 0f;
        Quaternion start = transform.localRotation;
        while(t < landingTime)
        {
            transform.localRotation = Quaternion.Slerp(start, Quaternion.identity, t / landingTime);
            yield return null;
            t += Time.deltaTime;
    }
    }
    public void EnableShipMovementAtLevelStart()
    {
        /*playerControllerscript.laserCooldown = false;
        playerControllerscript.ProcessWeaponFire(false, false);*/
        playerControllerscript.enabled = true;
        playerControllerscript.EnableLasers();
    }
    public void DisableShipMovementAtLevelEnd()
    {
        //playerControllerscript.shipControlsDisabled = true;
        //playerControllerscript.laserCooldown = false;
        //playerControllerscript.ProcessWeaponFire(false, false);
        //playerControllerscript.laserCooldown = true;
        //playerControllerscript.missileOnCooldown = true;
        playerControllerscript.DisableLasers();
        playerControllerscript.enabled = false;
        isLanding = true;
        StartCoroutine(EndLandingSequence());
        preLandingPosition = transform.localPosition;
        //preLandingRotation = transform.localRotation;
        //preLandingRotation = transform.localRotation.eulerAngles;
        xLandingVelocity = ((0f - preLandingPosition.x)/2f);//calculate landing velocity based on a 2 second landing
        yLandingVelocity = ((2.1f - preLandingPosition.y)/2f);//calculate landing velocity based on a 2 second landing
        //zLandingVelocity = ((0f - preLandingPosition.z)/2f);
        //xLandingRotationVelocity = ((0f - preLandingRotation.x)/2f);
        //yLandingRotationVelocity = ((0f - preLandingRotation.y)/2f);
        //zLandingRotationVelocity = ((0f - preLandingRotation.z)/2f);
        //Debug.Log("Prelanding position coordinates are: " + preLandingPosition.x + ", " + preLandingPosition.y);
        //Debug.Log("Landing velocities are: " + xLandingVelocity + ", " + yLandingVelocity);
        //Debug.Log("Prelanding rotation values are: " + preLandingRotation.x + ", " + preLandingRotation.y + ", " + preLandingRotation.z);
        //Debug.Log("Landing rotation velocities are: " + xLandingRotationVelocity + ", " + yLandingRotationVelocity + ", " + zLandingRotationVelocity);
        //transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        //transform.localPosition = new Vector3(0f, 2.5f, 0f);
        StartCoroutine(ProcessLandingRotation(landingTimeNeeded));
    }
    IEnumerator EndLandingSequence()
    {
        yield return new WaitForSeconds(2f);
        isLanding = false;
        //Debug.Log("Final postion at landing (before hard set) is: " + transform.localPosition.x + ", " + transform.localPosition.y);
        //Debug.Log("Final rotation at landing (before hard set) is: " + transform.localRotation.eulerAngles.x + ", " + transform.localRotation.eulerAngles.y + ", " + transform.localRotation.eulerAngles.z);
        transform.localPosition = new Vector3(0f, 2.1f, 0f);
        transform.localRotation = Quaternion.identity;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Debug.Log("Quaternion identity angles are: " + Quaternion.identity.x + ", " + Quaternion.identity.y + ", " + Quaternion.identity.z);
        //Debug.Log("Quaternion identity euler angles are: " + Quaternion.identity.eulerAngles.x + ", " + Quaternion.identity.eulerAngles.y + ", " + Quaternion.identity.eulerAngles.z);
    }
}
