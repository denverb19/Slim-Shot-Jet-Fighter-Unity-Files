using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    //[SerializeField] GameObject playerRig;
    [SerializeField] float controlSpeed = 10f;
    [SerializeField] float zRotationFactor = 0.01f;
    [SerializeField] float xRotationFactor = 0.0025f;
    [SerializeField] float yRotationFactor = 0.0025f;
    [SerializeField] float xRange = 11f;
    [SerializeField] float yRange = 4f;
    [Header("Rotation Settings")]
    [SerializeField] float zRotationRange = 60f;
    [SerializeField] float xRotationRange = 60f;
    [SerializeField] float yRotationRange = 25f;
    private float zRotationChange = 0f;
    [SerializeField] float evasionLength = 2f;
    [SerializeField] float evasionCooldownLength = 2f;
    [SerializeField] AudioClip evadeSoundClip;
    [Header("Weapon Settings")]
    [SerializeField] GameObject[] primaryLasers;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float missileCooldown = 2f;
    ParticleSystem primaryLasersLeft;
    ParticleSystem primaryLasersRight;
    ParticleSystem primaryOuterLasersLeft;
    ParticleSystem primaryOuterLasersRight;
    ParticleSystem primaryLasersTop;
    ParticleSystem primaryLasersBottom;
    ScoreBoard currentScoreBoard;
    //AudioSource leftLaserSounds;
    //AudioSource rightLaserSounds;

    public AudioSource playerAudioSource;
    //public AudioSource laserSounds;
    //public AudioSource leftLaserSounds;
    //public AudioSource rightLaserSounds;
    public bool isEvading = false;
    public bool evadingRight = true;
    public bool evadeOnCooldown = false;
    public bool missileOnCooldown = false;
    public bool laserCooldown = false;
    //public bool shipControlsDisabled = false;
    float rigYRotation = 0f;
    float cosOfYRotation = 0f;
    float sinOfYRotation = 0f;
    float missileXOffset = 0f;
    float missileZOffset = 0f;
    int currentUpgradeLevel = 0;
    ParticleSystem.EmissionModule leftEmitter;
    ParticleSystem.EmissionModule rightEmitter;
    ParticleSystem.EmissionModule leftOuterEmitter;
    ParticleSystem.EmissionModule rightOuterEmitter;
    ParticleSystem.EmissionModule topEmitter;
    ParticleSystem.EmissionModule bottomEmitter;
    //private bool laserSoundsOn = false;
    void Start()
    {
        primaryLasersLeft = primaryLasers[0].GetComponent<ParticleSystem>();
        primaryLasersRight = primaryLasers[1].GetComponent<ParticleSystem>();
        primaryOuterLasersLeft = primaryLasers[2].GetComponent<ParticleSystem>();
        primaryOuterLasersRight = primaryLasers[3].GetComponent<ParticleSystem>();
        primaryLasersTop= primaryLasers[4].GetComponent<ParticleSystem>();
        primaryLasersBottom = primaryLasers[5].GetComponent<ParticleSystem>();
        leftEmitter = primaryLasersLeft.emission;
        rightEmitter = primaryLasersRight.emission;
        leftOuterEmitter = primaryOuterLasersLeft.emission;
        rightOuterEmitter = primaryOuterLasersRight.emission;
        topEmitter = primaryLasersTop.emission;
        bottomEmitter = primaryLasersBottom.emission;
        StartCoroutine(GetScoreBoard());
        //leftLaserSounds = primaryLasers[0].GetComponent<AudioSource>();
        //rightLaserSounds = primaryLasers[1].GetComponent<AudioSource>();

        //playerAudioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool primaryFireInput = Input.GetButton("Fire1");
        bool secondaryFireInput = Input.GetButton("Fire2");
        bool evadeInput = Input.GetButton("Evade");
        ProcessEvasion(evadeInput, horizontalInput);
        ProcessPlayerMovement(horizontalInput, verticalInput);
        ProcessRotation(horizontalInput, verticalInput);
        //if(!shipControlsDisabled)
        //{
            ProcessWeaponFire(primaryFireInput, secondaryFireInput);
        //}
        //ProcessWeaponFire(primaryFireInput, secondaryFireInput);
    }
    IEnumerator GetScoreBoard()
    {
        yield return new WaitForSeconds(0.1f);
        currentScoreBoard = FindObjectOfType<ScoreBoard>();
    }
    void ProcessEvasion(bool evasionInput, float horizontalInput)
    {
        if(evasionInput && !isEvading && !evadeOnCooldown)
        {
            isEvading = true;
            if(horizontalInput >= 0f)
            {
                evadingRight = true;
            }
            else
            {
                evadingRight = false;
            }
            playerAudioSource.clip = evadeSoundClip;
            playerAudioSource.Play();
            StartCoroutine(EvadeRoll(horizontalInput));
        }
    }
    void ProcessPlayerMovement(float xInput, float yInput)
    {
        float xSpeed = xInput * Time.deltaTime * controlSpeed;
        float xOffset = transform.localPosition.x + xSpeed;
        float xChange = Mathf.Clamp( xOffset, -xRange, xRange );
        float ySpeed = yInput * Time.deltaTime * controlSpeed;
        float yOffset = transform.localPosition.y + ySpeed;
        float yChange = Mathf.Clamp( yOffset, -yRange, (3f * yRange) );
        transform.localPosition = new Vector3(
            xChange, yChange, transform.localPosition.z );
    }
    void ProcessRotation(float xInput, float yInput)
    {
        //float zRotationSpeed = xInput * 10000f * zRotationFactor;
        //float zRotationChange = Mathf.Clamp( zRotationSpeed, -zRotationRange, zRotationRange );
        //float zRotationChange = 0f;
        if(isEvading)
        {
            if(evadingRight)
            {
                //float zRotationSpeed = xInput * 10000f * zRotationFactor;
                //float zRotationChange = Mathf.Clamp( zRotationSpeed, -zRotationRange, zRotationRange );
                zRotationChange += 360f * Time.deltaTime;
            }
            else
            {
                zRotationChange -= 360f * Time.deltaTime;
            }
        }
        else
        {
            float zRotationSpeed = xInput * 10000f * zRotationFactor;
            zRotationChange = Mathf.Clamp( zRotationSpeed, -zRotationRange, zRotationRange );

            //float zRotationChange = Mathf.Clamp( zRotationSpeed, -zRotationRange, zRotationRange );
            //zRotationChange += zRotationSpeed;
            //float zRotationSpeed = xInput * 10000f * zRotationFactor * Time.deltaTime;
            //zRotationChange += Mathf.Clamp( zRotationSpeed, -zRotationRange, zRotationRange );  
            //zRotationChange = Mathf.Clamp( zRotationChange, -zRotationRange, zRotationRange );         
        }
        float yRotationSpeed = xInput * 10000f * yRotationFactor;
        float yRotationChange = Mathf.Clamp( yRotationSpeed, -yRotationRange, yRotationRange );
        float xRotationSpeed = yInput * 10000f * xRotationFactor;
        float xRotationChange = Mathf.Clamp( xRotationSpeed, -xRotationRange, xRotationRange );
        //transform.localRotation = Quaternion.Euler(-xRotationChange, 0f, -zRotationChange);
        transform.localRotation = Quaternion.Euler(-xRotationChange, yRotationChange, -zRotationChange);
    }
    public void ProcessWeaponFire(bool primaryFireInput, bool secondaryFireInput)
    {
        if(primaryFireInput && !laserCooldown)
        {
            StartCoroutine(SetFirePrimaryLasers(true));
        }
        else if(!laserCooldown)
        {
            StartCoroutine(SetFirePrimaryLasers(false));
        }
        if(secondaryFireInput && !missileOnCooldown)
        {
            if(currentScoreBoard.GetMissileCount() > 0)
            {
                StartCoroutine(FireMissile());
            }
            //StartCoroutine(FireMissile());
        }
    }
    //void SetFirePrimaryLasers(bool setFireOption)
    IEnumerator SetFirePrimaryLasers(bool setFireOption)
    {
        //var leftEmitter = primaryLasersLeft.emission;
        //var rightEmitter = primaryLasersRight.emission;
        //leftEmitter.enabled = setFireOption;
        //rightEmitter.enabled = setFireOption;
        laserCooldown = true;
        if(currentUpgradeLevel == 0)
        {
            leftEmitter.enabled = setFireOption;
            rightEmitter.enabled = setFireOption;
            leftOuterEmitter.enabled = false;
            rightOuterEmitter.enabled = false;
            topEmitter.enabled = false;
            bottomEmitter.enabled = false;
        }
        else if(currentUpgradeLevel == 1)
        {
           // var leftOuterEmitter = primaryOuterLasersLeft.emission;
            //var rightOuterEmitter = primaryOuterLasersRight.emission;
            leftEmitter.enabled = setFireOption;
            rightEmitter.enabled = setFireOption;
            leftOuterEmitter.enabled = setFireOption;
            rightOuterEmitter.enabled = setFireOption;
            topEmitter.enabled = false;
            bottomEmitter.enabled = false;
        }
        else if(currentUpgradeLevel == 2)
        {
            //var leftOuterEmitter = primaryOuterLasersLeft.emission;
            //var rightOuterEmitter = primaryOuterLasersRight.emission;
            //var topEmitter = primaryLasersTop.emission;
           // var bottomEmitter = primaryLasersBottom.emission;
            leftEmitter.enabled = setFireOption;
            rightEmitter.enabled = setFireOption;
            leftOuterEmitter.enabled = setFireOption;
            rightOuterEmitter.enabled = setFireOption;
            topEmitter.enabled = setFireOption;
            bottomEmitter.enabled = setFireOption;
        }
        yield return new WaitForSeconds(0.14f);
        laserCooldown = false;
        /*if(setFireOption && !laserSoundsOn)
        {
            laserSounds.Play();
            //rightLaserSounds.Play();
            laserSoundsOn = true;
        }
        else
        {
            laserSounds.Stop();
            //rightLaserSounds.Stop();
            laserSoundsOn = false;
        }*/
    }
    IEnumerator FireMissile()
    {
        missileOnCooldown = true;
        //GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        Quaternion missileRotation = transform.parent.gameObject.transform.rotation;
        missileRotation = Quaternion.Euler(
            missileRotation.eulerAngles.x, missileRotation.eulerAngles.y-90, missileRotation.eulerAngles.z);
        rigYRotation = transform.parent.transform.rotation.eulerAngles.y;
        cosOfYRotation = Mathf.Cos(rigYRotation * Mathf.Deg2Rad);
        sinOfYRotation = Mathf.Sin(rigYRotation * Mathf.Deg2Rad);
        missileXOffset = 2*cosOfYRotation;
        missileZOffset = 2*sinOfYRotation;
        Vector3 leftMissilePosition = new(transform.position.x+missileXOffset, transform.position.y-1, transform.position.z+missileZOffset);
        Vector3 rightMissilePosition = new(transform.position.x-missileXOffset, transform.position.y-1, transform.position.z-missileZOffset);
        //Vector3 leftMissilePosition = new(transform.position.x+2, transform.position.y-1, transform.position.z);
        //Vector3 rightMissilePosition = new(transform.position.x-2, transform.position.y-1, transform.position.z);
        //Vector3 missileRotationVector = new Vector3(missileRotation.x, missileRotation.y+90, missileRotation.z);
        //missileRotation = Quaternion.Euler(missileRotationVector);
        //missileRotation = new Quaternion(missileRotation.x, missileRotation.y, missileRotation.z, missileRotation.w);
        //GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.Euler(0, 270, 0));
        //GameObject missile = Instantiate(missilePrefab, transform.position, missileRotation);
        //Instantiate(missilePrefab, transform.position, missileRotation);
        //Instantiate(missilePrefab, missilePosition, missileRotation);
        Instantiate(missilePrefab, leftMissilePosition, missileRotation);
        Instantiate(missilePrefab, rightMissilePosition, missileRotation);
        //missile.transform.parent = playerRig.transform;
        currentScoreBoard.ChangeMissileColor(255, 0, 64, 255);
        currentScoreBoard.ChangeMissileCount(-2);
        yield return new WaitForSeconds(missileCooldown);
        currentScoreBoard.ChangeMissileColor(255, 255, 255, 255);
        missileOnCooldown = false;
    }
    IEnumerator EvadeRoll(float horizontalInput)
    {
        currentScoreBoard.ChangeEngineColor(0, 255, 128, 255);
        yield return new WaitForSeconds(evasionLength);
        currentScoreBoard.ChangeEngineColor(255, 0, 64, 255);
        isEvading = false;
        evadeOnCooldown = true;
        yield return new WaitForSeconds(evasionCooldownLength);
        currentScoreBoard.ChangeEngineColor(255, 255, 255, 255);
        evadeOnCooldown = false;
    }
    public void SetUpgradeLevel(int newUpgradeLevel)
    {
        currentUpgradeLevel = newUpgradeLevel;
    }
    public void DisableLasers()
    {
        for(int i = 0; i < primaryLasers.Length; i++)
        {
            primaryLasers[i].SetActive(false);
        }
        /*foreach ( GameObject laser in primaryLasers)
            {
                laser.GetComponent<>.enabled = false;
            }*/
    }
    public void EnableLasers()
    {
        for(int i = 0; i < primaryLasers.Length; i++)
        {
            primaryLasers[i].SetActive(true);
        }
        /*foreach ( GameObject laser in primaryLasers)
            {
                laser.gameObject.enabled = true;
            }*/
    }
}