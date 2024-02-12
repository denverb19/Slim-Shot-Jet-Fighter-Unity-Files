using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DestructSelf()
    {
        Destroy(gameObject, 0f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
