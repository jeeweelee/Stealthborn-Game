using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int score;
    private AudioSource audioSource;
    public AudioClip CrystalSFX;
    public const int WINNING_SCORE = 4;
    public TMP_Text objectiveText;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        score = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Crystal")) 
        {   
            score++;
            Destroy(collider.gameObject);
            audioSource.PlayOneShot(CrystalSFX);
            if (collectedAllCrystals())
                objectiveText.text = "Objective: Find the door and exit";
        } else if (collider.CompareTag("Enemy"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScreen");
        }

        
    }
    public bool collectedAllCrystals()
    {
        return score == WINNING_SCORE;
    }
    public int getScore()
    {
        return score;
    }
}
