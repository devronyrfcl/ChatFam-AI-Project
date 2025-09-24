using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSelector : MonoBehaviour
{
    [Header("Button Setup")]
    public Button[] buttons; // Assign buttons in inspector
    public Sprite normalSprite;
    public Sprite selectedSprite;

    [Header("Text Colors")]
    public Color normalTextColor = Color.white;
    public Color selectedTextColor = Color.yellow;

    [Header("Selected Value")]
    public string selectedValue; // Stores the chosen option

    // Internal references
    private Image[] buttonImages;
    private TextMeshProUGUI[] buttonTexts;

    [Tooltip("Match the number of buttons. Each value corresponds to a button.")]
    public string[] values;

    private void Start()
    {
        if (buttons.Length != values.Length)
        {
            Debug.LogError("❌ Buttons and Values must be the same length!");
            return;
        }

        buttonImages = new Image[buttons.Length];
        buttonTexts = new TextMeshProUGUI[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Local copy for closure
            buttonImages[i] = buttons[i].GetComponent<Image>();
            buttonTexts[i] = buttons[i].GetComponentInChildren<TextMeshProUGUI>();

            // Initialize sprites & text colors
            buttonImages[i].sprite = normalSprite;
            if (buttonTexts[i] != null)
                buttonTexts[i].color = normalTextColor;

            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void OnButtonClicked(int index)
    {
        // Reset all buttons
        for (int i = 0; i < buttonImages.Length; i++)
        {
            buttonImages[i].sprite = normalSprite;
            if (buttonTexts[i] != null)
                buttonTexts[i].color = normalTextColor;
        }

        // Set clicked button
        buttonImages[index].sprite = selectedSprite;
        if (buttonTexts[index] != null)
            buttonTexts[index].color = selectedTextColor;

        // Store selected value
        selectedValue = values[index];
    }

    // Call this function when you want to "proceed"
    public void Proceed()
    {
        if (string.IsNullOrEmpty(selectedValue))
        {
            Debug.LogWarning("⚠️ No value selected!");
        }
        else
        {
            Debug.Log("✅ Proceeding with value: " + selectedValue);
        }
    }
}
