using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class StaticAdsShower : MonoBehaviour
{
    [Header("Assign your Ad UI Panels here")]
    public GameObject[] ads; // All ad UI objects

    [Header("Progress Bar (Image with Fill)")]
    public Image progressBar; // UI Image fill type = Filled, fillMethod = Horizontal

    [Header("Timing Settings")]
    public float waitTime = 2f; // Time before switching to next ad
    public float transitionTime = 0.6f; // Time for smooth sliding
    public float offscreenX = -780f; // Start X position for hidden ad

    private int currentIndex = 0;

    void Start()
    {
        // Disable all ads except the first one
        for (int i = 0; i < ads.Length; i++)
        {
            if (i == 0) ads[i].SetActive(true);
            else ads[i].SetActive(false);
        }

        if (progressBar != null)
            progressBar.fillAmount = 0f; // start empty

        StartCoroutine(ShowAdsLoop());
    }

    IEnumerator ShowAdsLoop()
    {
        int adCount = ads.Length;

        while (true)
        {
            int nextIndex = (currentIndex + 1) % adCount;

            // Prepare next ad
            ads[nextIndex].SetActive(true);
            RectTransform nextRT = ads[nextIndex].GetComponent<RectTransform>();
            nextRT.anchoredPosition = new Vector2(offscreenX, nextRT.anchoredPosition.y);

            // Reset progress if looping back to first ad
            if (nextIndex == 0 && progressBar != null)
                progressBar.fillAmount = 0f;

            // Progress bar target fill value (step fraction)
            float targetFill = (float)nextIndex / adCount;

            // Animate progress bar over wait time
            if (progressBar != null)
                progressBar.DOFillAmount(targetFill, waitTime).SetEase(Ease.Linear);

            // Wait before sliding in
            yield return new WaitForSeconds(waitTime);

            // Slide next ad into view
            nextRT.DOAnchorPosX(0f, transitionTime).SetEase(Ease.OutCubic);

            // Disable current ad after transition
            yield return new WaitForSeconds(transitionTime);
            ads[currentIndex].SetActive(false);

            // Update index
            currentIndex = nextIndex;
        }
    }
}
