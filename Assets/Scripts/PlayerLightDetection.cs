using UnityEngine;

public class PlayerLightDetection : MonoBehaviour
{
    public Light directionalLight;     // The directional light to check
    public float darknessThreshold = 0.2f; // Threshold to consider the player in shadow

    // Check if the player is in shadow
    public bool IsInShadow()
    {
        if (directionalLight == null || directionalLight.type != LightType.Directional)
        {
            Debug.LogError("Directional light is not assigned or is not of type Directional.");
            return false; // Default to "not in shadow" if no directional light is set
        }

        // Offset the raycast origin slightly above the player to avoid hitting the ground or player collider
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.5f;

        // Calculate the light direction
        Vector3 lightDirection = -directionalLight.transform.forward;

        // Perform a raycast, excluding irrelevant layers (e.g., ground or player itself)
        int layerMask = LayerMask.GetMask("Default", "ShadowCasters"); // Only include relevant layers
        if (Physics.Raycast(raycastOrigin, lightDirection, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            // Check if the hit object is a valid shadow-caster
            if (hit.collider.CompareTag("ShadowCaster"))
            {
                Debug.Log($"Raycast hit shadow-casting object: {hit.collider.name}, player is in shadow.");
                return true; // Player is in shadow
            }
            else
            {
                Debug.Log($"Raycast hit non-shadow-casting object: {hit.collider.name}, treating as light.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything, player is in light.");
        }

        return false; // Player is in light
    }



}
