using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public string nextScene;
    public TMP_Text subtitleText;
    public bool isWinningScreen = false;
    public bool isLosingScreen = false;

    private string[] winningMessages = {
        "Winning: because losing just isn’t your style.",
        "You came, you saw, you conquered… and now you’re off to celebrate!",
        "Winning isn’t everything, but I bet it sure feels like it right now!"
    };

    private string[] losingMessages = { 
        "You played like a pro... a pro at losing!", 
        "If at first you don’t succeed, maybe this game isn’t for you.", 
        "Looks like you just unlocked the ‘Try Again’ achievement!", 
        "You fought bravely… and lost hilariously." 
    };


    // Start is called before the first frame update
    void Start()
    {
        if (isWinningScreen || isLosingScreen) {
            int randomIndex = UnityEngine.Random.Range(0, isWinningScreen ? winningMessages.Length : losingMessages.Length);
            subtitleText.text = isWinningScreen ? winningMessages[randomIndex] : losingMessages[randomIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //audioSource.PlayOneShot(startSFX);
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }
    }
}
