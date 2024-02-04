using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float controlSpeed = 10f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float xRange = 9f;
    [SerializeField] float yRange = 4f;
    [SerializeField] float zRotationRange = 75f;
    [SerializeField] float xRotationRange = 75f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //Debug.Log("horizontal input is: " + horizontalInput);
        //Debug.Log("vertical input is: " + verticalInput);
        ProcessPlayerMovement(horizontalInput, verticalInput);
        ProcessRotation(horizontalInput, verticalInput);
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
        float zRotationSpeed = xInput * 10000f * rotationSpeed;
        //float zRotationOffset = Mathf.Round(zRotationSpeed);
        //Debug.Log("zRotationOffset is: " + zRotationOffset);
        float zRotationChange = Mathf.Clamp( zRotationSpeed, -zRotationRange, zRotationRange );
        float xRotationSpeed = yInput * 10000f * rotationSpeed;
        //float xRotationOffset = Mathf.Round(xRotationSpeed);
        //Debug.Log("xRotationOffset is: " + xRotationOffset);
        float xRotationChange = Mathf.Clamp( xRotationSpeed, -xRotationRange, xRotationRange );
        transform.localRotation = Quaternion.Euler(-xRotationChange, 0f, -zRotationChange);
    }
}
