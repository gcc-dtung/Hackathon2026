using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherUI : MonoBehaviour
{
    [SerializeField] private GameObject weatherPanel;
    [SerializeField] private TextMeshProUGUI eventNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image weatherIcon;

    private void Start()
    {
        if (weatherPanel != null) weatherPanel.SetActive(false);

        if (WeatherManager.Instance != null)
        {
            WeatherManager.Instance.OnWeatherStart += ShowWeatherWarning;
            WeatherManager.Instance.OnWeatherEnd += HideWeatherWarning;
        }
    }

    private void OnDestroy()
    {
        if (WeatherManager.InstanceExists)
        {
            WeatherManager.Instance.OnWeatherStart -= ShowWeatherWarning;
            WeatherManager.Instance.OnWeatherEnd -= HideWeatherWarning;
        }
    }

    private void ShowWeatherWarning(WeatherEventData eventData)
    {
        if (weatherPanel != null)
        {
            weatherPanel.SetActive(true);
            
            if (eventNameText != null) eventNameText.text = eventData.eventName;
            if (descriptionText != null) descriptionText.text = eventData.description;
        }
    }

    private void HideWeatherWarning()
    {
        if (weatherPanel != null)
        {
            weatherPanel.SetActive(false);
        }
    }
}
