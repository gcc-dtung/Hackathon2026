using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;

    private void Update()
    {
        cam.transform.position = player.position;
    }
}
