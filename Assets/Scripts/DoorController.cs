using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{  
    public GameObject player;
    public string nextSceneName;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && player.GetComponent<PlayerController>().collectedAllCrystals())
        {
            Debug.Log("You touched this door");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}