using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float deathLength = 1f;
    [SerializeField] float damageTime = 0.5f;
    [SerializeField] GameObject enemyDeathExplosion;
    [SerializeField] GameObject enemyDamageVFX;
    //[SerializeField] Transform VFXparent;
    [SerializeField] int enemyValue = 5;
    [SerializeField] float enemyHealth = 3f;
    //private SelfDestruct selfDestructorScriptDamage;
    //private SelfDestruct selfDestructorScriptDeath;
    GameObject VFXparent;

    bool showingDamage = false;
    bool isDying = false;
    ScoreBoard currentScoreboard;
    Rigidbody thisRigidbody;
    AudioSource deathSound;
    //EnemyShooter thisShooterScript;
    void Start()
    {
        //StartCoroutine(GetScoreBoard());
        //currentScoreboard = FindObjectOfType<ScoreBoard>();
        //VFXparent = GameObject.FindWithTag("VFX Parent");
        thisRigidbody = gameObject.AddComponent<Rigidbody>();
        thisRigidbody.useGravity = false;
        deathSound = gameObject.GetComponent<AudioSource>();
        //thisShooterScript = gameObject.GetComponent<EnemyShooter>();
        //thisRigidbody.isKinematic = true;
        //selfDestructorScript = GetComponent<SelfDestruct>();
    }
    void OnParticleCollision(GameObject collidedObject)
    {
        ProcessHit(1f);
        //StartCoroutine(Deaththroes(deathLength));
        //Debug.Log(this.name + " was hit by: " + collidedObject.gameObject.name);
        //Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            //case "Enemy":
                //DeathSequence();
                //break;
            case "Terrain":
                DeathSequence();
                break;
            case "Player":
                DeathSequence();
                break;
            default:
                break;
        }
    }
    public void ProcessHit(float damageTaken)
    {
        enemyHealth -= damageTaken;
        if ( enemyHealth <= 0f && !isDying)
        {
            //isDying = true;
            currentScoreboard = FindObjectOfType<ScoreBoard>();
            currentScoreboard.IncreaseScore(enemyValue);
            //DisableShip();
            //StartCoroutine(Deaththroes(deathLength));
            DeathSequence();
        }
        else
        {
            StartCoroutine(ShowEnemyDamage(damageTime));
        }
    }
    void DisableShip()
    {
        //gameObject.GetComponent<Renderer>().enabled = false;
        Renderer[] shipPieces = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in shipPieces)
        {
            r.enabled = false;
        }
        //gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //EnemyShooter thisShooterScript = gameObject.GetComponent<EnemyShooter>();
        //thisShooterScript.SetDeath();
        //thisShooterScript.ResetCooldown();
    }
    void DeathSequence()
    {
        if (!isDying)
        {
            isDying = true;
            DisableShip();
            StartCoroutine(Deaththroes(deathLength));
        }
    }
    IEnumerator ShowEnemyDamage(float damageTime)
    {
        if (!showingDamage)
        {
            showingDamage = true;
            GameObject damageVFX = Instantiate(enemyDamageVFX, transform.position, Quaternion.identity);
            VFXparent = GameObject.FindWithTag("VFX Parent");
            damageVFX.transform.parent = VFXparent.transform;
            SelfDestruct selfDestructorScriptDamage = damageVFX.GetComponent<SelfDestruct>();
            yield return new WaitForSeconds(damageTime);
            selfDestructorScriptDamage.DestructSelf();
            showingDamage = false;    
        }
    }
    IEnumerator Deaththroes(float endLength)
    {
        GameObject deathVFX = Instantiate(enemyDeathExplosion, transform.position, Quaternion.identity);
        VFXparent = GameObject.FindWithTag("VFX Parent");
        deathVFX.transform.parent = VFXparent.transform;
        SelfDestruct selfDestructorScriptDeath = deathVFX.GetComponent<SelfDestruct>();
        deathSound.Play();
        //EnemyShooter thisShooterScript = gameObject.GetComponent<EnemyShooter>().ResetCooldown();
        
        //enemyDeathExplosion.Play();
        yield return new WaitForSeconds(endLength);
        selfDestructorScriptDeath.DestructSelf();
        /*if(thisShooterScript != null)
        {
            thisShooterScript.ResetCooldown();
        }*/
        //thisShooterScript.ResetCooldown();
        //gameObject.GetComponent<EnemyShooter>().ResetCooldown();
        //thisShooterScript.SetDeath();
        Destroy(gameObject);
    }
    /*IEnumerator GetScoreBoard()
    {
        //playerControllerscript = GetComponent<PlayerController>();
        
        yield return new WaitForSeconds(1f);
        //healthSliderScript = FindObjectOfType<HealthSlider>();
        //healthSliderScript.UpdateHealth(playerCurrentHealth/playerHealthMaximum);
        currentScoreboard = FindObjectOfType<ScoreBoard>();
        VFXparent = GameObject.FindWithTag("VFX Parent");
    }*/
    public float GetHealth()
    {
        return enemyHealth;
    }
}
