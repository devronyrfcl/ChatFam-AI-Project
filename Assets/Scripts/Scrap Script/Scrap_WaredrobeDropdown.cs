using UnityEngine;
using TMPro;

public class Scrap_WaredrobeDropdown : MonoBehaviour
{
    [Header("Dropdown Reference")]
    public TMP_Dropdown wardrobeDropdown;

    [Header("Panels")]
    public GameObject clothesPanel;
    public GameObject appearancePanel;
    public GameObject roomPanel;

    private void Start()
    {
        if (wardrobeDropdown != null)
        {
            wardrobeDropdown.onValueChanged.AddListener(OnDropdownChanged);
            UpdatePanel(wardrobeDropdown.value); // initialize the correct panel on start
        }
    }

    private void OnDestroy()
    {
        // Clean up listener
        if (wardrobeDropdown != null)
            wardrobeDropdown.onValueChanged.RemoveListener(OnDropdownChanged);
    }

    private void OnDropdownChanged(int index)
    {
        UpdatePanel(index);
    }

    private void UpdatePanel(int index)
    {
        // Disable all panels first
        clothesPanel.SetActive(false);
        appearancePanel.SetActive(false);
        roomPanel.SetActive(false);

        // Enable the panel based on dropdown selection
        switch (index)
        {
            case 0: // Clothes
                clothesPanel.SetActive(true);
                break;
            case 1: // Appearance
                appearancePanel.SetActive(true);
                break;
            case 2: // Room
                roomPanel.SetActive(true);
                break;
            default:
                Debug.LogWarning("Dropdown index out of range");
                break;
        }
    }
}
