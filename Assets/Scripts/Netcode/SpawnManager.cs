using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using KartGame.KartSystems;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance; // Singleton for global access
    private List<Transform> spawnPoints; // List to hold spawn positions
    private int nextSpawnIndex = 0;      // Index to track the next available spawn point

    public List<GameObject> vehiclePrefabs;   // List of vehicle prefabs
    public List<GameObject> characterPrefabs;

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

        // Initialize spawn points
        spawnPoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
    }

    private void Start()
    {
        Debug.Log("V�hicule s�lectionn� : " + GameData.SelectedVehicleIndex);
        Debug.Log("perso s�lectionn� : " + GameData.SelectedCharacterIndex);
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
            // Server handles vehicle spawning
            Transform spawnPoint = GetNextSpawnPoint();
            // Debug.Log("spawnPoint.position, spawnPoint.rotation " + spawnPoint.position + " " + spawnPoint.rotation);
            // GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            // player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            GameObject selectedVehicle = GetVehiclePrefab(GameData.SelectedVehicleIndex);
            GameObject selectedCharacter = GetCharacterPrefab(GameData.SelectedCharacterIndex);

            if (selectedVehicle == null || selectedCharacter == null)
            {
                Debug.LogError("Impossible de trouver le prefab du v�hicule s�lectionn�. Assurez-vous que l'indice est correct.");
                return;
            }

            // Instantiate the selected vehicle directly
            GameObject vehicle = Instantiate(selectedVehicle, spawnPoint.position, spawnPoint.rotation);

            // Spawn the vehicle as a networked object
            vehicle.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            AttachCharacterToVehicle(vehicle, selectedCharacter);
        }
    }

    public Transform GetNextSpawnPoint()
    {
        if (nextSpawnIndex >= spawnPoints.Count)
        {
            Debug.LogWarning("No more spawn points available! Repeating from the start.");
            nextSpawnIndex = 0;
        }
        return spawnPoints[nextSpawnIndex++];
    }

    private GameObject GetVehiclePrefab(int index)
    {
        if (index >= 0 && index < vehiclePrefabs.Count)
        {
            return vehiclePrefabs[index];
        }
        Debug.LogWarning($"Vehicle index {index} is out of range.");
        return null;
    }

    private GameObject GetCharacterPrefab(int index)
    {
        if (index >= 0 && index < characterPrefabs.Count)
        {
            return characterPrefabs[index];
        }
        Debug.LogWarning($"Character index {index} is out of range.");
        return null;
    }

    private void AttachCharacterToVehicle(GameObject vehicle, GameObject characterPrefab)
    {
        // Find a mount point on the vehicle (ensure you have a "CharacterMountPoint" transform on the vehicle prefab)
        Transform mountPoint = vehicle.transform.Find("CharacterMountPoint");

        if (mountPoint != null)
        {
            // Instantiate the character at the mount point's position
            GameObject character = Instantiate(characterPrefab, mountPoint.position, mountPoint.rotation);
            character.transform.SetParent(mountPoint); // Parent the character to the vehicle
        }
        else
        {
            Debug.LogWarning("CharacterMountPoint not found on vehicle. Make sure your vehicle prefab has a transform named 'CharacterMountPoint'.");
        }
    }
}
