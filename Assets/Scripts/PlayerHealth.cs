using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float delayDesired = 2f;
    [SerializeField] ParticleSystem playerDeathExplosion;
    private PlayerController playerControllerscript;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerscript = GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                CrashSequence();
                break;
            case "Terrain":
                CrashSequence();
                break;
            default:
                break;
        }
    }
    void CrashSequence()
    {
        if (playerControllerscript.enabled == true)
        {
            playerControllerscript.enabled = false;
            Renderer[] shipPieces = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in shipPieces)
            {
                r.enabled = false;
            }
            playerDeathExplosion.GetComponent<Renderer>().enabled = true;
            playerDeathExplosion.Play();
            StartCoroutine(ReloadLevel(delayDesired));
        }
    }
    IEnumerator ReloadLevel(float delayRequired)
    {
        yield return new WaitForSeconds(delayRequired);
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }
}
