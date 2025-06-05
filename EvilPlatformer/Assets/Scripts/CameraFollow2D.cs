using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;               // Assign your player’s Transform here
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);

    private void LateUpdate()
    {
        if (target == null) return;

        // Directly snap the camera to the target position + offset
        transform.position = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            offset.z
        );
    }
}
