using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance; // Singleton for global access
    private List<Transform> spawnPoints; // List to hold spawn positions
    private int nextSpawnIndex = 0;      // Index to track the next available spawn point
    public GameObject playerPrefab;     // Player prefab

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        spawnPoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
    }

    private void Start()
    {
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
        if (NetworkManager.Singleton.IsServer)
        {
            // Server handles player spawning
            Transform spawnPoint = GetNextSpawnPoint();
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }

    private Transform GetNextSpawnPoint()
    {
        if (nextSpawnIndex >= spawnPoints.Count)
        {
            Debug.LogWarning("No more spawn points available! Repeating from the start.");
            nextSpawnIndex = 0;
        }
        return spawnPoints[nextSpawnIndex++];
    }

    private GameObject FindPlayerObject(ulong clientId)
    {
        foreach (var networkObject in NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.transform.root.GetComponentsInChildren<NetworkObject>())
        {
            if (networkObject.IsOwner) return networkObject.gameObject;
        }
        return null;
    }
}
