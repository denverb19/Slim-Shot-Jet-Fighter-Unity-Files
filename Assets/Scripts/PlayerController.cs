using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
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
    [Header("Weapon Settings")]
    [SerializeField] GameObject[] primaryLasers;
    ParticleSystem primaryLasersLeft;
    ParticleSystem primaryLasersRight;
    void Start()
    {
        primaryLasersLeft = primaryLasers[0].GetComponent<ParticleSystem>();
        primaryLasersRight = primaryLasers[1].GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool primaryFireInput = Input.GetButton("Fire1");
        ProcessPlayerMovement(horizontalInput, verticalInput);
        ProcessRotation(horizontalInput, verticalInput);
        ProcessWeaponFire(primaryFireInput);
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
        float zRotationSpeed = xInput * 10000f * zRotationFactor;
        float zRotationChange = Mathf.Clamp( zRotationSpeed, -zRotationRange, zRotationRange );
        float yRotationSpeed = xInput * 10000f * yRotationFactor;
        float yRotationChange = Mathf.Clamp( yRotationSpeed, -yRotationRange, yRotationRange );
        float xRotationSpeed = yInput * 10000f * xRotationFactor;
        float xRotationChange = Mathf.Clamp( xRotationSpeed, -xRotationRange, xRotationRange );
        //transform.localRotation = Quaternion.Euler(-xRotationChange, 0f, -zRotationChange);
        transform.localRotation = Quaternion.Euler(-xRotationChange, yRotationChange, -zRotationChange);
    }
    void ProcessWeaponFire(bool primaryFireInput)
    {
        if(primaryFireInput)
        {
            SetFirePrimaryLasers(true);
        }
        else
        {
            SetFirePrimaryLasers(false);
        }
    }
    void SetFirePrimaryLasers(bool setFireOption)
    {
        var leftEmitter = primaryLasersLeft.emission;
        var rightEmitter = primaryLasersRight.emission;
        leftEmitter.enabled = setFireOption;
        rightEmitter.enabled = setFireOption;
    }

}
