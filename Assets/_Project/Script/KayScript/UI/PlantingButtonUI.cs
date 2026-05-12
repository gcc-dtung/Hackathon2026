using UnityEngine;
using UnityEngine.UI;

public class PlantingButtonUI : MonoBehaviour
{
    [SerializeField] private SeedPlacer seedPlacer;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color activeColor = Color.green;

    private void Update()
    {
        if (seedPlacer != null && buttonImage != null)
        {
            buttonImage.color = seedPlacer.IsPlantingMode ? activeColor : normalColor;
        }
    }
}
