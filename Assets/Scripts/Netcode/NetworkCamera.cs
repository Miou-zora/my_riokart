using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using System.Linq;

public class NetworkCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private int retryCount = 0; // Counter to track retry attempts
    private const int maxRetries = 10; // Maximum number of retry attempts
    private const float retryInterval = 0.2f; // Time interval between retries

    private void Start()
    {
        // Check if we have the virtual camera assigned in the inspector
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera not assigned in the inspector.");
            return;
        }

        // Register to the event when a player is spawned
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            // Start retrying to set the camera after a short delay
            InvokeRepeating(nameof(TrySetCameraToFollowLocalPlayer), 0.1f, retryInterval);
        }
    }

    private void TrySetCameraToFollowLocalPlayer()
    {
        // Attempt to find and set the camera to follow the local player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log("Got " + players.Count() + " players");
        foreach (GameObject player in players)
        {
            NetworkBehaviour networkBehaviour = player.GetComponent<NetworkBehaviour>();

            if (networkBehaviour != null && networkBehaviour.IsOwner)
            {
                // Set the virtual camera to follow and look at the local player
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
                Debug.Log("Camera now following the local player.");

                // Stop retrying once the camera has been set successfully
                CancelInvoke(nameof(TrySetCameraToFollowLocalPlayer));
                return;
            }
        }

        // Increment retry count and log a warning if retries are exhausted
        retryCount++;
        if (retryCount >= maxRetries)
        {
            Debug.LogWarning("Failed to find local player after multiple attempts. Ensure player prefab has the 'Player' tag.");
            CancelInvoke(nameof(TrySetCameraToFollowLocalPlayer));
        }
    }
}
