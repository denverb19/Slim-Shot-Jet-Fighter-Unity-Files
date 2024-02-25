using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject primaryLaser;
    [SerializeField] float primaryLaserCooldown;
    [SerializeField] bool isPrimaryOnCooldown = false;
    [SerializeField] bool resetInMotion = false;
    //[SerializeField] bool isDead = false;
    float cosOfYRotation = 0f;
    float sinOfYRotation = 0f;
    float laserXOffset = 0f;
    float laserZOffset = 0f;
    float initialYRotation = 0f;
    [SerializeField] float offsetSize = 25f;
    //EnemyHealth thisHealthScript;
    float currentHealth = 3f;
    void Start()
    {
        //isPrimaryOnCooldown = false;
        //thisHealthScript = gameObject.GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        /*currentHealth = thisHealthScript.GetHealth();
        if(currentHealth < 1f)
        {
            isPrimaryOnCooldown = false;
        }
        else if(!isPrimaryOnCooldown)
        {
            StartCoroutine(FirePrimaryLaser());
        }*/
        if(!isPrimaryOnCooldown)
        {
            //isPrimaryOnCooldown = true;
            StartCoroutine(FirePrimaryLaser());
        }
        else if(!resetInMotion)
        {
            resetInMotion = true;
            StartCoroutine(ResetCooldown());
            //isPrimaryOnCooldown = false;
        }
    }
    IEnumerator FirePrimaryLaser()
    {
        isPrimaryOnCooldown = true;
        Quaternion laserRotation = transform.rotation;
        laserRotation = Quaternion.Euler(
            laserRotation.eulerAngles.x, laserRotation.eulerAngles.y-90, laserRotation.eulerAngles.z-90);
        //Instantiate(primaryLaser, transform.position, transform.rotation);
        initialYRotation = transform.rotation.eulerAngles.y;
        cosOfYRotation = Mathf.Cos(initialYRotation * Mathf.Deg2Rad);
        sinOfYRotation = Mathf.Sin(initialYRotation * Mathf.Deg2Rad);
        laserXOffset = offsetSize*cosOfYRotation;
        laserZOffset = offsetSize*sinOfYRotation;
        Vector3 leftLaserPosition = new(transform.position.x+laserXOffset, transform.position.y, transform.position.z+laserZOffset);
        Vector3 rightLaserPosition = new(transform.position.x-laserXOffset, transform.position.y, transform.position.z-laserZOffset);
        Vector3 topLaserPosition = new(transform.position.x, transform.position.y+offsetSize, transform.position.z);
        Vector3 bottomLaserPosition = new(transform.position.x, transform.position.y-offsetSize, transform.position.z);
        Instantiate(primaryLaser, transform.position, laserRotation);//central laser
        Instantiate(primaryLaser, leftLaserPosition, laserRotation);//left laser
        Instantiate(primaryLaser, rightLaserPosition, laserRotation);//right laser
        Instantiate(primaryLaser, topLaserPosition, laserRotation);//top laser
        Instantiate(primaryLaser, bottomLaserPosition, laserRotation);//bottom laser
        yield return new WaitForSeconds(primaryLaserCooldown);
        isPrimaryOnCooldown = false;
    }
    IEnumerator ResetCooldown()
    {
        //resetInMotion = true;
        yield return new WaitForSeconds(2f);
        isPrimaryOnCooldown = false;
        resetInMotion = false;
    }
    /*public void SetDeath()
    {
        isDead = !isDead;
    }*/
}
