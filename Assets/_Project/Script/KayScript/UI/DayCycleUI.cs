using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private Image timeProgressFill;

    private void Start()
    {
        if (DayCycleManager.Instance != null)
        {
            DayCycleManager.Instance.OnNewDay += UpdateDayText;
            DayCycleManager.Instance.OnTimeChanged += UpdateTimeProgress;
            
            UpdateDayText(DayCycleManager.Instance.CurrentDay);
        }
    }

    private void OnDestroy()
    {
        if (DayCycleManager.InstanceExists)
        {
            DayCycleManager.Instance.OnNewDay -= UpdateDayText;
            DayCycleManager.Instance.OnTimeChanged -= UpdateTimeProgress;
        }
    }

    private void UpdateDayText(int day)
    {
        if (dayText != null)
        {
            dayText.text = $"Day {day}";
        }
    }

    private void UpdateTimeProgress(float normalizedTime)
    {
        if (timeProgressFill != null)
        {
            timeProgressFill.fillAmount = normalizedTime;
        }
    }
}
