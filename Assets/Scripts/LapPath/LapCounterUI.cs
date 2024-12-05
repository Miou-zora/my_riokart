using UnityEngine;
using TMPro;

public class LapCounterUI : MonoBehaviour
{
    public TextMeshProUGUI lapCounter;
    private GameObject player;

    private int lastLapCheck = 0;

    // Public method to set the player
    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
        UpdateLapCounter(); // Update UI immediately once the player is set
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            int currentLap = player.GetComponent<Player>().currentLap;
            if (lastLapCheck != currentLap)
            {
                UpdateLapCounter();
                lastLapCheck = currentLap;
            }
        }
    }

    // Helper method to update lap counter text
    private void UpdateLapCounter()
    {
        if (player != null)
        {
            lapCounter.text = "Lap: " + (player.GetComponent<Player>().currentLap + 1).ToString() + "/3";
        }
    }
}
