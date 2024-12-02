using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LogoSelector : MonoBehaviour
{
    public GameObject characterSelectionCanvas;
    public GameObject vehicleSelectionCanvas;
    public GameObject courseSelectionCanvas;

    public enum MenuType { Character, Vehicle, Circuit }
    public MenuType currentMenu = MenuType.Character;

    public List<RectTransform> characters;
    public string[] characterNames;
    public TextMeshProUGUI characterNameText;
    private int selectedCharacterIndex = 0;
    public int columns = 2;
    public Image characterImageInVehicleMenu;
    public List<Image> characterImages;

    public List<GameObject> vehicles;
    public string[] vehicleNames;
    public TextMeshProUGUI vehicleNameText;
    private int selectedVehicleIndex = 0;
    public int vehicleColumns = 2;
    public List<Image> vehicleImages;
    public Image characterImageInCourseMenu;
    public Image vehicleImageInCourseMenu;

    public List<RectTransform> logos;
    public float normalScale = 1.0f;
    public float selectedScale = 1.2f;
    private int selectedIndex = 0;
    public string[] sceneNames;
 

    void Start()
    {
        UpdateLogoSizes();
        if (currentMenu == MenuType.Character)
        {
            UpdateCharacterSelection();
        }
        else if (currentMenu == MenuType.Vehicle)
        {
            UpdateVehicleSelection();
        }
    }

    void Update()
    {
        if (currentMenu == MenuType.Circuit)
        {
            HandleCircuitSelection();
        }
        else if (currentMenu == MenuType.Character)
        {
            HandleCharacterSelection();
        }
        else if (currentMenu == MenuType.Vehicle)
        {
            HandleVehicleSelection();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchMenu();
        }
    }

    void HandleCircuitSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = (selectedIndex + 1) % logos.Count;
            UpdateLogoSizes();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex = (selectedIndex - 1 + logos.Count) % logos.Count;
            UpdateLogoSizes();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 4 + logos.Count) % logos.Count;
            UpdateLogoSizes();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 4) % logos.Count;
            UpdateLogoSizes();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadSelectedScene();
        }
    }

    void HandleCharacterSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedCharacterIndex = (selectedCharacterIndex + 1) % characters.Count;
            UpdateCharacterSelection();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedCharacterIndex = (selectedCharacterIndex - 1 + characters.Count) % characters.Count;
            UpdateCharacterSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedCharacterIndex = (selectedCharacterIndex - columns + characters.Count) % characters.Count;
            UpdateCharacterSelection();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedCharacterIndex = (selectedCharacterIndex + columns) % characters.Count;
            UpdateCharacterSelection();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (characterImages != null && selectedCharacterIndex < characterImages.Count)
            {
                characterImageInVehicleMenu.sprite = characterImages[selectedCharacterIndex].sprite;
                characterImageInCourseMenu.sprite = characterImages[selectedCharacterIndex].sprite;
            }
            SwitchMenu();
        }
    }

    void HandleVehicleSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedVehicleIndex = (selectedVehicleIndex + 1) % vehicles.Count;
            UpdateVehicleSelection();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedVehicleIndex = (selectedVehicleIndex - 1 + vehicles.Count) % vehicles.Count;
            UpdateVehicleSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedVehicleIndex = (selectedVehicleIndex - vehicleColumns + vehicles.Count) % vehicles.Count;
            UpdateVehicleSelection();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedVehicleIndex = (selectedVehicleIndex + vehicleColumns) % vehicles.Count;
            UpdateVehicleSelection();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (vehicleImages != null && selectedVehicleIndex < vehicleImages.Count)
            {
                vehicleImageInCourseMenu.sprite = vehicleImages[selectedVehicleIndex].sprite;
            }
            SwitchMenu();
        }
    }

    void LoadSelectedScene()
    {
        if (selectedIndex < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[selectedIndex]);
        }
        else
        {
            Debug.LogWarning("Aucune scène assignée pour ce logo !");
        }
    }

    void UpdateCharacterSelection()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (i == selectedCharacterIndex)
            {
                characters[i].transform.localScale = Vector3.one * selectedScale;
            }
            else
            {
                characters[i].transform.localScale = Vector3.one * normalScale;
            }
        }

        if (characterImages != null && selectedCharacterIndex < characterImages.Count)
        {
            characterImageInVehicleMenu.sprite = characterImages[selectedCharacterIndex].sprite;
            characterImageInCourseMenu.sprite = characterImages[selectedCharacterIndex].sprite;
        }
        if (characterNameText != null && selectedCharacterIndex < characterNames.Length)
        {
            characterNameText.text = characterNames[selectedCharacterIndex];
        }
    }

    void UpdateVehicleSelection()
    {
        for (int i = 0; i < vehicles.Count; i++)
        {
            if (i == selectedVehicleIndex)
            {
                vehicles[i].transform.localScale = Vector3.one * selectedScale;
            }
            else
            {
                vehicles[i].transform.localScale = Vector3.one * normalScale;
            }
        }

        if (vehicleNameText != null && selectedVehicleIndex < vehicleNames.Length)
        {
            vehicleNameText.text = vehicleNames[selectedVehicleIndex];
        }
    }

    void UpdateLogoSizes()
    {
        for (int i = 0; i < logos.Count; i++)
        {
            if (i == selectedIndex)
                logos[i].localScale = Vector3.one * selectedScale;
            else
                logos[i].localScale = Vector3.one * normalScale;
        }
    }

    void SwitchMenu()
    {
        if (currentMenu == MenuType.Circuit)
        {
            currentMenu = MenuType.Character;
            characterSelectionCanvas.SetActive(true);
            vehicleSelectionCanvas.SetActive(false);
            courseSelectionCanvas.SetActive(false);
            UpdateCharacterSelection();
        }
        else if (currentMenu == MenuType.Character)
        {
            currentMenu = MenuType.Vehicle;
            characterSelectionCanvas.SetActive(false);
            vehicleSelectionCanvas.SetActive(true);
            courseSelectionCanvas.SetActive(false);
            UpdateVehicleSelection();
            GameData.SelectedCharacterName = characterNames[selectedCharacterIndex];
        }
        else if (currentMenu == MenuType.Vehicle)
        {
            currentMenu = MenuType.Circuit;
            characterSelectionCanvas.SetActive(false);
            vehicleSelectionCanvas.SetActive(false);
            courseSelectionCanvas.SetActive(true);
            UpdateLogoSizes();

            GameData.SelectedVehicleName = vehicleNames[selectedVehicleIndex];
        }
    }
}
