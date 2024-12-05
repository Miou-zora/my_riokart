using Unity.Netcode;
using UnityEngine;

public class Synchronization : NetworkBehaviour
{
    public GameObject playerPrefab;
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();
    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>();

    public float smoothSpeed = 5f;

    private void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("The player prefab is not set under the Synchronization script. No sync possible.");
            return;
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            UpdatePositionServerRpc(transform.position, transform.rotation);
        }

        if (!IsOwner)
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition.Value, Time.deltaTime * smoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation.Value, Time.deltaTime * smoothSpeed);
        }
    }

    [ServerRpc]
    private void UpdatePositionServerRpc(Vector3 newPosition, Quaternion newRotation)
    {
        networkPosition.Value = newPosition;
        networkRotation.Value = newRotation;

        SyncPositionClientRpc(newPosition, newRotation);
    }

    [ClientRpc]
    private void SyncPositionClientRpc(Vector3 newPosition, Quaternion newRotation)
    {
        if (!IsOwner)
        {
            transform.position = newPosition;
            transform.rotation = newRotation;
        }
    }
}
