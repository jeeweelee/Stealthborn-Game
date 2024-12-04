using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int score;
    private AudioSource audioSource;
    public AudioClip CrystalSFX;

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
        }

        
    }
    public int getScore()
    {
        return score;
    }
}
