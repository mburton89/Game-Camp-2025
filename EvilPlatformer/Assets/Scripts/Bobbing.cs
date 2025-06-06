using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Bobbing : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [Tooltip("Maximum vertical offset from the starting position.")]
    public float amplitude = 1f;

    [Tooltip("Speed of the bobbing effect.")]
    public float frequency = 1f;

    // Internal
    private Vector3 startPosition;

    private void Start()
    {
        // Cache the starting world position
        startPosition = transform.position;
    }

    private void Update()
    {
        // Calculate vertical offset via a sine wave: Sin( time * freq ) * amplitude
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;

        // Apply the offset to the original position
        transform.position = startPosition + Vector3.up * offsetY;
    }
}
