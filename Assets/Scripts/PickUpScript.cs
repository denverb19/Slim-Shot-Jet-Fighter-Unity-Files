using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    // Start is called before the first frame update
    float rotationRate = 0f;
    [SerializeField] float rotationIncrement = 2f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationRate += rotationIncrement;
        rotationRate %= 360f; 
        ProcessRotation(rotationRate);
    }
    public void DestroyPickup()
    {
        Destroy(gameObject, 0f);
    }
    void ProcessRotation(float rotationAmount)
    {
        transform.localRotation = Quaternion.Euler(0f, rotationAmount, 0f);
    }
}
