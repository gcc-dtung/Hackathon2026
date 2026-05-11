using UnityEngine;

public enum TreeState
{
    Sapling,
    Growing,
    Mature
}

public class PlantedTree : MonoBehaviour
{
    [Header("Growth Settings")]
    [SerializeField] private float growthTimeSeconds = 60f;
    [SerializeField] private Vector3 saplingScale = new Vector3(0.2f, 0.2f, 0.2f);
    [SerializeField] private Vector3 matureScale = Vector3.one;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    private float _currentGrowth = 0f;
    private float _health;
    private TreeState _state = TreeState.Sapling;

    public TreeState State => _state;

    private void Start()
    {
        _health = maxHealth;
        transform.localScale = saplingScale;
        
        if (TreeManager.Instance != null)
        {
            TreeManager.Instance.RegisterTree(this);
        }
    }

    private void Update()
    {
        if (_state != TreeState.Mature)
        {
            Grow();
        }
    }

    private void Grow()
    {
        _currentGrowth += Time.deltaTime;
        
        float progress = Mathf.Clamp01(_currentGrowth / growthTimeSeconds);
        transform.localScale = Vector3.Lerp(saplingScale, matureScale, progress);

        if (progress >= 0.5f && _state == TreeState.Sapling)
        {
            _state = TreeState.Growing;
        }
        else if (progress >= 1f && _state != TreeState.Mature)
        {
            _state = TreeState.Mature;
        }
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (TreeManager.Instance != null)
        {
            TreeManager.Instance.UnregisterTree(this);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (TreeManager.Instance != null)
        {
            TreeManager.Instance.UnregisterTree(this);
        }
    }
}
