using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float oxygenPerSecond = 10f;
    [SerializeField] private Image oxygenBar;

    private float _oxygen;

    void Start()
    {
        _oxygen = maxOxygen;
    }

    void Update()
    {
        _oxygen -= oxygenPerSecond * Time.deltaTime;
    }
    void FixedUpdate()
    {
        oxygenBar.fillAmount = _oxygen / maxOxygen;
        if(_oxygen <= 0){}
        //TODO: Die method
    }
}
