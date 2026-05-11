using System;
using UnityEngine;

public class DayCycleManager : Singleton<DayCycleManager>
{
    [Header("Settings")]
    [SerializeField] private float dayDurationInSeconds = 300f; // 5 mins
    [SerializeField] private int maxDays = 7;
    [SerializeField] private Light directionalLight;
    
    private int _currentDay = 1;
    private float _currentTimeInSeconds = 0f;
    private bool _isCycleActive = true;

    public int CurrentDay => _currentDay;
    public float NormalizedTime => _currentTimeInSeconds / dayDurationInSeconds;
    public bool IsNight => NormalizedTime > 0.6f && NormalizedTime < 0.9f;

    public event Action<int> OnNewDay;
    public event Action<float> OnTimeChanged;
    public event Action OnFinalDayEnd;

    private void Update()
    {
        if (!_isCycleActive) return;

        _currentTimeInSeconds += Time.deltaTime;
        
        // 0 to 1
        float normalized = _currentTimeInSeconds / dayDurationInSeconds;
        OnTimeChanged?.Invoke(normalized);

        UpdateLighting(normalized);

        if (_currentTimeInSeconds >= dayDurationInSeconds)
        {
            AdvanceDay();
        }
    }

    private void AdvanceDay()
    {
        _currentDay++;
        _currentTimeInSeconds = 0f;

        if (_currentDay > maxDays)
        {
            _isCycleActive = false;
            OnFinalDayEnd?.Invoke();
        }
        else
        {
            OnNewDay?.Invoke(_currentDay);
        }
    }

    private void UpdateLighting(float normalizedTime)
    {
        if (directionalLight != null)
        {
            // Sun rotates from 0 to 360 degrees based on normalized time.
            // 0.25 = sunrise, 0.5 = noon, 0.75 = sunset. Adjust as needed.
            float sunAngle = (normalizedTime * 360f) - 90f; 
            directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        }
    }
    
    public void PauseCycle() => _isCycleActive = false;
    public void ResumeCycle() => _isCycleActive = true;
}
