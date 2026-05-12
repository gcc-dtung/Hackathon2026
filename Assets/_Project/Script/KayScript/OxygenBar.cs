using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float baseDepletionRate = 10f;
    [SerializeField] private float reductionFactor = 0.15f; // How much each tree slows depletion
    [SerializeField] private Image oxygenBar;

    public event System.Action OnOxygenDepleted;
    public event System.Action<float, float> OnOxygenChanged;

    private float _oxygen;

    private System.Action _showGameOverAction;

    private void OnEnable()
    {
        _showGameOverAction = GameOverUI.Instance.ShowGameOver;
        OnOxygenDepleted += _showGameOverAction;
    }

    void Start()
    {
        _oxygen = maxOxygen;
    }

    void Update()
    {
        if(_oxygen <= 0)
        {
            return;
        }
        float treeCount = 0;
        if (TreeManager.Instance != null)
        {
            treeCount = TreeManager.Instance.TreeCount;
        }

        // Formula: actualRate = baseRate / (1 + treeCount * reductionFactor)
        float actualDepletionRate = baseDepletionRate / (1f + (treeCount * reductionFactor));
        
        _oxygen -= actualDepletionRate * Time.deltaTime;
        _oxygen = Mathf.Clamp(_oxygen, 0, maxOxygen);
        
        OnOxygenChanged?.Invoke(_oxygen, maxOxygen);

        if (_oxygen <= 0)
        {
            Debug.Log("Die");
            OnOxygenDepleted?.Invoke();
        }
    }
    
    void FixedUpdate()
    {
        if (oxygenBar != null)
        {
            oxygenBar.fillAmount = _oxygen / maxOxygen;
        }
    }

    void OnDisable()
    {
        if (_showGameOverAction != null)
        {
            OnOxygenDepleted -= _showGameOverAction;
        }
    }

    public void AddOxygen(float amount)
    {
        _oxygen += amount;
        _oxygen = Mathf.Clamp(_oxygen, 0, maxOxygen);
        OnOxygenChanged?.Invoke(_oxygen, maxOxygen);
    }
}
