using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[System.Serializable]
public class StringEvent : UnityEvent<string> { }

public class ButtonSelector : MonoBehaviour
{
    [Header("Button Setup")]
    public Button[] buttons;
    public Sprite normalSprite;
    public Sprite selectedSprite;

    [Header("Text Colors")]
    public Color normalTextColor = Color.white;
    public Color selectedTextColor = Color.yellow;

    [Header("Selected Value")]
    public string selectedValue;

    [Header("Events")]
    public StringEvent OnValueSelected; // Assign in inspector

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
            int index = i;
            buttonImages[i] = buttons[i].GetComponent<Image>();
            buttonTexts[i] = buttons[i].GetComponentInChildren<TextMeshProUGUI>();

            buttonImages[i].sprite = normalSprite;
            if (buttonTexts[i] != null)
                buttonTexts[i].color = normalTextColor;

            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void OnButtonClicked(int index)
    {
        for (int i = 0; i < buttonImages.Length; i++)
        {
            buttonImages[i].sprite = normalSprite;
            if (buttonTexts[i] != null)
                buttonTexts[i].color = normalTextColor;
        }

        buttonImages[index].sprite = selectedSprite;
        if (buttonTexts[index] != null)
            buttonTexts[index].color = selectedTextColor;

        selectedValue = values[index];

        // 🔥 Notify listeners
        OnValueSelected.Invoke(selectedValue);
    }

    public void Proceed()
    {
        if (string.IsNullOrEmpty(selectedValue))
            Debug.LogWarning("⚠️ No value selected!");
        else
            Debug.Log("✅ Proceeding with value: " + selectedValue);
    }
}
