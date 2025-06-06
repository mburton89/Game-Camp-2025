using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class Slime : MonoBehaviour
{
    [Header("Bounce Settings")]
    [Tooltip("The upward force (Impulse) applied to any Rigidbody2D that enters this trigger.")]
    public float bounceForce = 20f;

    [Header("Sprites")]
    [Tooltip("Sprite to display after this bouncer has been used.")]
    public Sprite inactiveSprite;

    // Internal state
    private bool hasBounced = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D triggerCollider;

    private void Awake()
    {
        // Cache references
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerCollider = GetComponent<Collider2D>();

        // Ensure this collider is set as a trigger
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBounced) return; // Already used, do nothing

        // Try to get the Rigidbody2D on the other object
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Apply a strong upward impulse
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

            // Mark as used, disable further bounces, and swap sprite
            hasBounced = true;
            triggerCollider.enabled = false;

            if (inactiveSprite != null)
            {
                spriteRenderer.sprite = inactiveSprite;
            }
        }

        SoundManager.Instance.PlaySound("bounce");
    }
}
