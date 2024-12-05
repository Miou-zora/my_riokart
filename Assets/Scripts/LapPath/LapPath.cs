using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapPath : MonoBehaviour
{
    List<Transform> subPath = new List<Transform>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            subPath.Add(child);
        }
    }

    void Update()
    {

    }

    // OnTrigger for child lap path
    public void OnTriggerEnterChildPath(Collider triggerCollider, ChildPath childPath)
    {
        if (triggerCollider.CompareTag("PlayerCollider") || triggerCollider.CompareTag("CPUCollider")) {
            Player player = triggerCollider.GetComponentInParent<Player>();
            int childIndex = childPath.transform.GetSiblingIndex();

            if (player != null && player.currentCheckpoint != childIndex && childIndex - player.currentCheckpoint == 1) {
                Debug.Log("Current Checkpoint: " + player.currentCheckpoint);
                player.currentCheckpoint = childIndex;
            }
        }
    }

    // OnTrigger parent lap path
    public void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.CompareTag("PlayerCollider") || triggerCollider.CompareTag("CPUCollider")) {
            Player player = triggerCollider.GetComponentInParent<Player>();

            if (player != null && player.currentCheckpoint == subPath.Count() - 1) {
                Debug.Log("Current Lap: " + player.currentLap);
                player.currentLap += 1;
                player.currentCheckpoint = 0;
            }
        }
    }
}
