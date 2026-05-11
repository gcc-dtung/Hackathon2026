using UnityEngine;

public enum WeatherType
{
    AcidRain,
    DustStorm,
    TrashRain
}

[CreateAssetMenu(fileName = "New Weather Event", menuName = "Breathe/Weather Event")]
public class WeatherEventData : ScriptableObject
{
    public string eventName;
    public WeatherType weatherType;
    [TextArea] public string description;
    
    public float duration = 30f;
    public float damagePerSecond = 0f; // For Acid Rain
    public float oxygenDrainMultiplier = 1f; // For Dust Storm
    
    [Range(1, 7)] public int minDay = 1;
    [Range(1, 7)] public int maxDay = 7;
    [Range(0f, 1f)] public float probability = 0.5f;

    public Color ambientColor = Color.white;
    public float fogDensity = 0.01f;
}
