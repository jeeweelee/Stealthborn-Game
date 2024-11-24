using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class ShadowDetection : MonoBehaviour
{
    public Light directionalLight; 
    public TextMeshProUGUI shadowText; 

    private void Start()
    {
        if (directionalLight == null)
        {
            Debug.LogError("Directional light is not assigned!");
        }

        if (shadowText == null)
        {
            Debug.LogError("ShadowText (TMP) is not assigned!");
        }
        else
        {
            shadowText.text = ""; 
        }
    }

    private void Update()
    {
        CheckShadow();
    }

    private void CheckShadow()
    {
        if (directionalLight == null || shadowText == null) return;

        Vector3 directionToLight = -directionalLight.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToLight, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject != this.gameObject)
            {
                shadowText.text = "In Shadow";
                return;
            }
        }

        shadowText.text = "Not In Shadow";
    }
}
