using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControls : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float controlSpeed = 200f;
    [SerializeField] float missileLifeTime = 3f;
    [SerializeField] int missileDamage = 30;
    float yRotation = 0f;
    float cosOfYRotation = 0f;
    float sinOfYRotation = 0f;
    void Start()
    {
        yRotation = transform.rotation.eulerAngles.y;
        cosOfYRotation = Mathf.Cos(yRotation * Mathf.Deg2Rad);
        sinOfYRotation = Mathf.Sin(yRotation * Mathf.Deg2Rad);
        StartCoroutine(MissileTimeout(missileLifeTime));
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
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                other.gameObject.GetComponent<EnemyHealth>().ProcessHit(missileDamage);
                //StartCoroutine(MissileTimeout(0f));
                Destroy(gameObject, 0f);
                break;
            case "Terrain":
                //StartCoroutine(MissileTimeout(0f));
                Destroy(gameObject, 0f);
                //StartCoroutine(TakeDamage(1f));
                //CrashSequence();
                break;
            default:
                break;
        }
    }
    IEnumerator MissileTimeout(float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject, 0f);
    }

}
