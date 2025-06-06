using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;               // Assign your player’s Transform here
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);

    [Header("Dead Zone Settings")]
    [Tooltip("The horizontal and vertical radius (in world units) around the camera center within which the camera will not move.")]
    public Vector2 deadZone = new Vector2(1f, 1f);

    private void LateUpdate()
    {
        if (target == null) return;

        // Compute the position the camera “wants” to be at, based on target + offset
        float desiredX = target.position.x + offset.x;
        float desiredY = target.position.y + offset.y;

        // Current camera position
        Vector3 currentPos = transform.position;

        // Compute distance from camera to desired position, on each axis
        float deltaX = desiredX - currentPos.x;
        float deltaY = desiredY - currentPos.y;

        float newX = currentPos.x;
        float newY = currentPos.y;

        // Only move on X if outside the horizontal dead zone
        if (Mathf.Abs(deltaX) > deadZone.x)
        {
            // Move just enough so that target sits on the dead zone boundary
            newX = desiredX - Mathf.Sign(deltaX) * deadZone.x;
        }

        // Only move on Y if outside the vertical dead zone
        if (Mathf.Abs(deltaY) > deadZone.y)
        {
            // Move just enough so that target sits on the dead zone boundary
            newY = desiredY - Mathf.Sign(deltaY) * deadZone.y;
        }

        // Update camera position; keep the fixed Z offset
        transform.position = new Vector3(newX, newY, offset.z);
    }
}
