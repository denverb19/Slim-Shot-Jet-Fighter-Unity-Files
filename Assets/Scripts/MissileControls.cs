using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControls : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float controlSpeed = 200f;
    float yRotation = 0f;
    float cosOfYRotation = 0f;
    float sinOfYRotation = 0f;
    void Start()
    {
        yRotation = transform.rotation.eulerAngles.y;
        cosOfYRotation = Mathf.Cos(yRotation * Mathf.Deg2Rad);
        sinOfYRotation = Mathf.Sin(yRotation * Mathf.Deg2Rad);
        //Debug.Log("Missiles zRotation is: " + yRotation);
        //Debug.Log("Cosine of zRotation is: " + cosOfYRotation);
        //Debug.Log("Sine of zRotation is: " + sinOfYRotation);
    }

    // Update is called once per frame
    void Update()
    {
        float missileSpeed = Time.deltaTime * controlSpeed;
        //float missileOffset = transform.localPosition.z + missileSpeed;
        //transform.localPosition = new Vector3(
            //transform.localPosition.x, transform.localPosition.y, missileOffset );
        //transform.localPosition = new Vector3(
            //transform.localPosition.x + Mathf.Cos(transform.rotation.eulerAngles.y)*missileSpeed, transform.localPosition.y,
            //transform.localPosition.z + Mathf.Sin(transform.rotation.eulerAngles.y)*missileSpeed );
        transform.localPosition = new Vector3(
            transform.localPosition.x + cosOfYRotation*missileSpeed, transform.localPosition.y,
            transform.localPosition.z + sinOfYRotation*missileSpeed*(-1) );
    }
}
