using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public const int PAUSE_VOLUME = -10;
    public GameObject pausedText;

    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            AudioListener.volume = isPaused ? 0f : 1f;
            Time.timeScale = isPaused ? 0 : 1;
            pausedText.SetActive(isPaused);
        }
    }
}
