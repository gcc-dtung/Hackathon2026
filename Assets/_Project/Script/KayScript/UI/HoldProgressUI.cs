using UnityEngine;
using UnityEngine.UI;

public class HoldProgressUI : MonoBehaviour
{
    [SerializeField] private HoldInteraction holdInteraction;
    [SerializeField] private Image progressFillImage;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        if (holdInteraction != null)
        {
            holdInteraction.OnHoldProgress += UpdateProgress;
            holdInteraction.OnInteractionComplete += HideProgress;
        }

        if (progressFillImage != null)
        {
            progressFillImage.fillAmount = 0f;
        }
        
        SetVisibility(false);
    }

    private void OnDestroy()
    {
        if (holdInteraction != null)
        {
            holdInteraction.OnHoldProgress -= UpdateProgress;
            holdInteraction.OnInteractionComplete -= HideProgress;
        }
    }

    private void UpdateProgress(float progress)
    {
        if (progressFillImage != null)
        {
            progressFillImage.fillAmount = progress;
        }

        SetVisibility(progress > 0f);
    }

    private void HideProgress()
    {
        if (progressFillImage != null)
        {
            progressFillImage.fillAmount = 0f;
        }
        SetVisibility(false);
    }

    private void SetVisibility(bool visible)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
        }
        else
        {
            gameObject.SetActive(visible);
        }
    }
}
