using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += UpdateCoinText;
            UpdateCoinText(CurrencyManager.Instance.Coins);
        }
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged -= UpdateCoinText;
        }
    }

    private void UpdateCoinText(int currentCoins)
    {
        if (coinText != null)
        {
            coinText.text = currentCoins.ToString();
        }
    }
}
