using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : Singleton<WeatherManager>
{
    [SerializeField] private List<WeatherEventData> availableEvents;
    
    private WeatherEventData _currentEvent;
    private float _eventTimer;

    public WeatherEventData CurrentEvent => _currentEvent;
    public event System.Action<WeatherEventData> OnWeatherStart;
    public event System.Action OnWeatherEnd;

    private void Start()
    {
        if (DayCycleManager.Instance != null)
        {
            DayCycleManager.Instance.OnNewDay += HandleNewDay;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (DayCycleManager.InstanceExists)
        {
            DayCycleManager.Instance.OnNewDay -= HandleNewDay;
        }
    }

    private void Update()
    {
        if (_currentEvent != null)
        {
            _eventTimer -= Time.deltaTime;
            ApplyWeatherEffects();

            if (_eventTimer <= 0)
            {
                EndWeatherEvent();
            }
        }
    }

    private void HandleNewDay(int dayNumber)
    {
        // Evaluate if a weather event should happen today
        List<WeatherEventData> possibleEvents = new List<WeatherEventData>();
        
        foreach (var evt in availableEvents)
        {
            if (dayNumber >= evt.minDay && dayNumber <= evt.maxDay)
            {
                possibleEvents.Add(evt);
            }
        }

        if (possibleEvents.Count > 0)
        {
            // Simple random selection
            WeatherEventData selectedEvent = possibleEvents[Random.Range(0, possibleEvents.Count)];
            if (Random.value <= selectedEvent.probability)
            {
                StartWeatherEvent(selectedEvent);
            }
        }
    }

    private void StartWeatherEvent(WeatherEventData eventData)
    {
        _currentEvent = eventData;
        _eventTimer = eventData.duration;
        
        RenderSettings.fog = true;
        RenderSettings.fogDensity = eventData.fogDensity;
        RenderSettings.ambientLight = eventData.ambientColor;

        OnWeatherStart?.Invoke(eventData);
    }

    private void EndWeatherEvent()
    {
        _currentEvent = null;
        RenderSettings.fog = false;
        OnWeatherEnd?.Invoke();
    }

    private void ApplyWeatherEffects()
    {
        // E.g., Acid Rain damaging plants
        if (_currentEvent.weatherType == WeatherType.AcidRain && _currentEvent.damagePerSecond > 0)
        {
            // In a real scenario, get all trees and apply damage.
            // For performance, this should be handled carefully or using a coroutine ticking every second.
        }
    }
}
