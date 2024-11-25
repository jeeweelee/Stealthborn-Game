using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{   public const int WINNING_SCORE = 4;
    public GameObject player;    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && player.GetComponent<PlayerController>().getScore() == WINNING_SCORE)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
        }
    }
}