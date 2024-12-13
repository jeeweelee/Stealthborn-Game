using System.Collections;
using UnityEngine;

public class BombAI : MonoBehaviour
{
    public string playerTag = "Player";
    public float sightRange = 10f;
    private float delayBeforeDie = 1.75f;
    private Animator animator;
    private Transform player;
    private bool isTriggered = false;
    private AudioSource audioSource;
    public AudioClip ScreamSFX;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        player = playerObject.transform;

    }

    void Update()
    {
        if (isTriggered) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= sightRange)
        {
            animator.SetTrigger("attack01");
            audioSource.PlayOneShot(ScreamSFX);
            isTriggered = true;
            StartCoroutine(TriggerPlayerDieAfterDelay());
        }
    }

    IEnumerator TriggerPlayerDieAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeDie); 
        if (player != null)
        {
            player.GetComponent<PlayerController>().Die(); 
        }
    }
}
