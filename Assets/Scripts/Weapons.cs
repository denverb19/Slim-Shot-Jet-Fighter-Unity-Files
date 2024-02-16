using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource weaponSound;
    bool weaponSoundOn = false;
    [SerializeField] float weaponSoundLength = 1f;
    void Start()
    {
        weaponSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool primaryFireInput = Input.GetButton("Fire1");
        weaponSoundDecision(primaryFireInput);
        //StartCoroutine(ProcessWeaponFireSounds(primaryFireInput));
    }
    void weaponSoundDecision(bool primaryFireInput)
    {
        if(primaryFireInput && !weaponSoundOn)
        {
            StartCoroutine(ProcessWeaponFireSounds(primaryFireInput));
        }
    }
    IEnumerator ProcessWeaponFireSounds(bool primaryFireInput)
    {
        //if(primaryFireInput && !weaponSoundOn)
        //{
        weaponSound.Play();
        weaponSoundOn = true;
        //Debug.Log("reached weapon fire sound on");
        //}
        //else
        //{
            //weaponSound.Stop();
            //weaponSoundOn = false;
            //Debug.Log("reached weapon fire sound off");
        //}
        yield return new WaitForSeconds(weaponSoundLength);
        weaponSound.Stop();
        weaponSoundOn = false;
        //Debug.Log("reached weapon fire sound off");
    }
}
