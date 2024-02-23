using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    // Start is called before the first frame update
    Slider healthSlider;
    void Awake()
    {
        healthSlider = gameObject.GetComponent<Slider>();
    }
    
    public void UpdateHealth(float newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, 1);
        healthSlider.value = newHealth;
    }void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
