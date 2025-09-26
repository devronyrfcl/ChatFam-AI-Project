using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
//unity scene management
using UnityEngine.SceneManagement;


public class ScrapScript_Slider2 : MonoBehaviour
{
    [Header("UI Setup")]
    public Image fillImage;
    public TextMeshProUGUI percentageText;
    public GameObject finalPage;

    [Header("Final Panel Setup")]
    public Image finalImage;
    public Button finalButton;
    public TextMeshProUGUI finalText;

    [Header("Cycling Texts (4 for smoothness)")]
    public TextMeshProUGUI topText;
    public TextMeshProUGUI middleText;
    public TextMeshProUGUI bottomText;
    public TextMeshProUGUI extraText;

    [Header("Settings")]
    public float stepValue = 0.1f;
    public float tweenDuration = 0.3f;
    public float cycleDuration = 1.0f;
    public float autoIncreaseStep = 0.05f;

    [Header("Current Value")]
    [Range(0f, 1f)]
    public float currentValue = 0f;

    [Header("String Values")]
    public string[] values;

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public Image loadingBar;

    // Internals
    private RectTransform topRT, middleRT, bottomRT, extraRT;
    private CanvasGroup topCG, middleCG, bottomCG, extraCG;
    private Sequence cycleSequence;
    private Tween percentageTween;
    private System.Random rng = new System.Random();

    private void Awake()
    {
        topRT = topText?.rectTransform;
        middleRT = middleText?.rectTransform;
        bottomRT = bottomText?.rectTransform;
        extraRT = extraText?.rectTransform;

        topCG = GetOrAddCanvasGroup(topText);
        middleCG = GetOrAddCanvasGroup(middleText);
        bottomCG = GetOrAddCanvasGroup(bottomText);
        extraCG = GetOrAddCanvasGroup(extraText);

        // Initialize final panel elements
        if (finalImage != null)
        {
            finalImage.transform.localScale = Vector3.one * 1f;
            finalImage.color = new Color(finalImage.color.r, finalImage.color.g, finalImage.color.b, 0.1f);
        }
        if (finalButton != null)
        {
            RectTransform btnRT = finalButton.GetComponent<RectTransform>();
            btnRT.anchoredPosition = new Vector2(btnRT.anchoredPosition.x, -900f);
        }
        if (finalText != null)
        {
            RectTransform txtRT = finalText.GetComponent<RectTransform>();
            txtRT.anchoredPosition = new Vector2(txtRT.anchoredPosition.x, -480f);

            CanvasGroup txtCG = finalText.GetComponent<CanvasGroup>();
            if (txtCG == null) txtCG = finalText.gameObject.AddComponent<CanvasGroup>();
            txtCG.alpha = 0f; // start fully transparent
        }


        if (finalPage != null)
            finalPage.SetActive(false);
    }

    private void Start()
    {
        if (fillImage == null) fillImage = GetComponent<Image>();
        currentValue = Mathf.Clamp01(currentValue);
        fillImage.fillAmount = currentValue;
        UpdatePercentageInstant(currentValue);

        ResetTextPositions();
        AssignRandomValue(topText);
        AssignRandomValue(middleText);
        AssignRandomValue(bottomText);
        AssignRandomValue(extraText);
        
    }

    #region Slider Controls
    public void ValueUp() => SetValue(currentValue + stepValue);
    public void ValueDown() => SetValue(currentValue - stepValue);

    public void SetValue(float newValue)
    {
        newValue = Mathf.Clamp01(newValue);
        float oldValue = currentValue;
        currentValue = newValue;

        fillImage.DOFillAmount(newValue, tweenDuration).SetEase(Ease.OutQuad);

        if (percentageTween != null && percentageTween.IsActive()) percentageTween.Kill();
        percentageTween = DOTween.To(() => oldValue, x => UpdatePercentageInstant(x), newValue, tweenDuration).SetEase(Ease.OutQuad);

        if (Mathf.Approximately(newValue, 1f))
        {
            DOVirtual.DelayedCall(2f, () =>
            {
                Debug.Log("🎉 Percentage reached 100%!");
                ActivateFinalPanel();
            });
        }
    }

    private void UpdatePercentageInstant(float normalizedValue)
    {
        if (percentageText != null)
        {
            float percent = normalizedValue * 100f;
            percentageText.text = percent.ToString("0.0") + "%";
        }
    }
    #endregion

    #region Final Panel Animation
    private void ActivateFinalPanel()
    {
        if (finalPage != null)
            finalPage.SetActive(true);

        if (finalImage != null)
        {
            finalImage.transform.DOScale(1f, 1f).SetEase(Ease.OutBack); // <- fix here
            finalImage.DOFade(1f, 1f); // this is fine
        }

        if (finalButton != null)
        {
            RectTransform btnRT = finalButton.GetComponent<RectTransform>();
            btnRT.DOAnchorPosY(-600f, 1f).SetEase(Ease.OutBack);
        }

        if (finalText != null)
        {
            RectTransform txtRT = finalText.GetComponent<RectTransform>();
            CanvasGroup txtCG = finalText.GetComponent<CanvasGroup>();

            txtRT.DOAnchorPosY(-280f, 1f).SetEase(Ease.OutBack); // move up
            txtCG.DOFade(1f, 1f); // fade in alpha from 0 → 1
        }

    }
    #endregion

    #region Text cycle helpers
    private CanvasGroup GetOrAddCanvasGroup(TextMeshProUGUI tmp)
    {
        if (tmp == null) return null;
        CanvasGroup cg = tmp.GetComponent<CanvasGroup>();
        if (cg == null) cg = tmp.gameObject.AddComponent<CanvasGroup>();
        return cg;
    }

    private void ResetTextPositions()
    {
        if (topRT != null) { topRT.anchoredPosition = new Vector2(topRT.anchoredPosition.x, -210f); topRT.localScale = Vector3.one * 0.7f; if (topCG) topCG.alpha = 1f; }
        if (middleRT != null) { middleRT.anchoredPosition = new Vector2(middleRT.anchoredPosition.x, -292f); middleRT.localScale = Vector3.one * 1f; if (middleCG) middleCG.alpha = 1f; }
        if (bottomRT != null) { bottomRT.anchoredPosition = new Vector2(bottomRT.anchoredPosition.x, -374f); bottomRT.localScale = Vector3.one * 0.7f; if (bottomCG) bottomCG.alpha = 1f; }
        if (extraRT != null) { extraRT.anchoredPosition = new Vector2(extraRT.anchoredPosition.x, -456f); extraRT.localScale = Vector3.one * 0.7f; if (extraCG) extraCG.alpha = 0f; }
    }

    public void StartCycle()
    {
        if (cycleSequence != null && cycleSequence.IsActive()) cycleSequence.Kill();
        cycleSequence = DOTween.Sequence();

        if (bottomRT != null)
        {
            cycleSequence.Join(bottomRT.DOAnchorPosY(-292f, cycleDuration).SetEase(Ease.OutQuad));
            cycleSequence.Join(bottomRT.DOScale(1f, cycleDuration).SetEase(Ease.OutQuad));
            if (bottomCG != null) cycleSequence.Join(bottomCG.DOFade(1f, cycleDuration));
        }

        if (middleRT != null)
        {
            cycleSequence.Join(middleRT.DOAnchorPosY(-210f, cycleDuration).SetEase(Ease.OutQuad));
            cycleSequence.Join(middleRT.DOScale(0.7f, cycleDuration).SetEase(Ease.OutQuad));
        }

        if (topRT != null)
        {
            cycleSequence.Join(topRT.DOAnchorPosY(-130f, cycleDuration).SetEase(Ease.OutQuad));
            cycleSequence.Join(topRT.DOScale(0.7f, cycleDuration).SetEase(Ease.OutQuad));
            if (topCG != null) cycleSequence.Join(topCG.DOFade(0f, cycleDuration));
        }

        if (extraRT != null)
        {
            if (extraCG != null) extraCG.alpha = 0f;
            cycleSequence.Join(extraRT.DOAnchorPosY(-374f, cycleDuration).SetEase(Ease.OutQuad));
            cycleSequence.Join(extraRT.DOScale(0.7f, cycleDuration).SetEase(Ease.OutQuad));
            if (extraCG != null) cycleSequence.Join(extraCG.DOFade(1f, cycleDuration));
        }

        cycleSequence.OnComplete(() =>
        {
            RotateReferences();
            if (extraRT != null) { extraRT.anchoredPosition = new Vector2(extraRT.anchoredPosition.x, -456f); extraRT.localScale = Vector3.one * 0.7f; }
            if (extraCG != null) extraCG.alpha = 0f;
            AssignRandomValue(extraText);
            SetValue(Mathf.Clamp01(currentValue + autoIncreaseStep));
            StartCycle();
        });

        cycleSequence.Play();
    }

    private void RotateReferences()
    {
        var oldTopText = topText;
        var oldTopRT = topRT;
        var oldTopCG = topCG;

        topText = middleText; topRT = middleRT; topCG = middleCG;
        middleText = bottomText; middleRT = bottomRT; middleCG = bottomCG;
        bottomText = extraText; bottomRT = extraRT; bottomCG = extraCG;
        extraText = oldTopText; extraRT = oldTopRT; extraCG = oldTopCG;
    }

    private void AssignRandomValue(TextMeshProUGUI target)
    {
        if (target == null || values == null || values.Length == 0) return;
        int idx = rng.Next(values.Length);
        target.text = values[idx];
    }
    #endregion

    //when button is clicked it will load the scene name MainApp. add some fake loadinmg filling then real loading
    public void OnFinalButtonClicked()
    {

        //final text will go up and fade out
        if (finalText != null)
        {
            RectTransform txtRT = finalText.GetComponent<RectTransform>();
            CanvasGroup txtCG = finalText.GetComponent<CanvasGroup>();
            txtRT.DOAnchorPosY(200f, 0.5f).SetEase(Ease.InBack); // move up
            txtCG.DOFade(0f, 0.5f); // fade out alpha from 1 → 0
        }
        //button will go down and fade out
        if (finalButton != null)
        {
            RectTransform btnRT = finalButton.GetComponent<RectTransform>();
            CanvasGroup btnCG = finalButton.GetComponent<CanvasGroup>();
            if (btnCG == null) btnCG = finalButton.gameObject.AddComponent<CanvasGroup>();
            btnRT.DOAnchorPosY(-1200f, 0.5f).SetEase(Ease.InBack);
            btnCG.DOFade(0f, 0.5f);
        }
        if (loadingScreen != null)
            loadingScreen.SetActive(true);
        if (loadingBar != null)
            loadingBar.fillAmount = 0f;
        //fake loading
        float fakeLoadDuration = 2f;
        DOTween.To(() => 0f, x => {
            if (loadingBar != null)
                loadingBar.fillAmount = x;
        }, 1f, fakeLoadDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            //after fake loading is done, load the real scene
            SceneManager.LoadScene("MainApp 2");
        });
    }

}
