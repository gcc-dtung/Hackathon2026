using UnityEngine;
using UnityEngine.EventSystems;

public class SeedPlacer : MonoBehaviour
{
    [SerializeField] private PlantedTree treePrefab;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float placeRange = 4f;
    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        // Simple tap to plant, checking if we hold seed
        var pointer = UnityEngine.InputSystem.Pointer.current;
        if (pointer != null && pointer.press.wasPressedThisFrame)
        {
            // Ignore UI touches
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            TryPlantSeed(pointer.position.ReadValue());
        }
    }

    private void TryPlantSeed(Vector2 screenPos)
    {
        if (playerCamera == null || treePrefab == null || Backpack.Instance == null) return;

        // Check if we have seeds
        if (!Backpack.Instance.HasItem(ResourceType.Seed, 1)) return;

        Ray ray = playerCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, placeRange, groundLayer))
        {
            // Consume seed
            if (Backpack.Instance.RemoveItem(ResourceType.Seed, 1))
            {
                // Spawn tree
                Instantiate(treePrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
