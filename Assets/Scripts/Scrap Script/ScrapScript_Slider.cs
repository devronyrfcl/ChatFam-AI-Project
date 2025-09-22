using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScrapScript_Slider : MonoBehaviour
{
    [Header("Image Fill Setup")]
    public Image fillImage;            // Assign a UI Image (set type to Filled)
    public float stepValue = 0.1f;     // Amount to add/remove (0–1 range)
    public float tweenDuration = 0.3f; // Smooth transition speed

    [Header("Current Value")]
    [Range(0f, 1f)]
    public float currentValue = 0f;    // Keeps track of the current fill (0–1)

    private void Start()
    {
        if (fillImage == null)
            fillImage = GetComponent<Image>();

        // Initialize fill
        currentValue = Mathf.Clamp01(currentValue);
        fillImage.fillAmount = currentValue;
    }

    /// <summary>
    /// Increase fill by stepValue.
    /// </summary>
    public void ValueUp()
    {
        SetValue(currentValue + stepValue);
    }

    /// <summary>
    /// Decrease fill by stepValue.
    /// </summary>
    public void ValueDown()
    {
        SetValue(currentValue - stepValue);
    }

    /// <summary>
    /// Set fill to specific value with tween.
    /// </summary>
    public void SetValue(float newValue)
    {
        // Clamp between 0 and 1
        newValue = Mathf.Clamp01(newValue);

        currentValue = newValue;

        // Animate with DOTween
        fillImage.DOFillAmount(newValue, tweenDuration).SetEase(Ease.OutQuad);
    }
}
