using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public int currentCheckpoint = 0;
    public int currentLap = 0;

    private GameObject player;
    private LapCounterUI lapCounterUi;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure this runs only for the local player.
        if (!IsLocalPlayer) return;

        player = gameObject;

        // Each player gets their own LapCounterUI instance.
        lapCounterUi = GameObject.FindGameObjectWithTag("PreExistingLapCounter").GetComponent<LapCounterUI>();
        
        LapCounterUI localUi = Instantiate(lapCounterUi, transform);
        if (lapCounterUi)
        {
            lapCounterUi.SetPlayer(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer) return;

        // Handle updates for the local player's UI if needed.
    }
}
