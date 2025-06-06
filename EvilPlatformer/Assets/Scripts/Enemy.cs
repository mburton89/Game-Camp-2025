using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed, bobDistance, bobForce;
    public bool isFacingLeft = true, isGoingUp = true, collideWithGround;
    public Transform leftPatrol, rightPatrol;
    private Vector2 originalPosition, previousFramePosition;
    public Rigidbody2D rb;
    private SpriteRenderer sr;
    private float flipCooldown = 0.5f;
    private float nextFlipTime = 0f;


    void Start()
    {
        originalPosition = transform.position;
        previousFramePosition = transform.position;

        // Cache SpriteRenderer
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer not found on Enemy!");
        }
    }

    void FixedUpdate()
    {
        Move();
        previousFramePosition = transform.position;
    }

    void Move()
    {
        PatrolRoute();
        BobCheck();

        if (isFacingLeft)
        {
            rb.AddForce(Vector2.left * speed * Time.fixedDeltaTime);
        }
        else
        {
            rb.AddForce(Vector2.right * speed * Time.fixedDeltaTime);
        }

        if (isGoingUp)
        {
            rb.AddForce(Vector2.up * bobForce * Time.fixedDeltaTime);
        }
        else
        {
            rb.AddForce(Vector2.down * bobForce * Time.fixedDeltaTime);
        }
    }

    void PatrolRoute()
    {
        bool stuck = previousFramePosition == (Vector2)transform.position;

        if (Time.time >= nextFlipTime)
        {
            if (isFacingLeft && (transform.position.x < leftPatrol.position.x || collideWithGround || stuck))
            {
                isFacingLeft = false;
                FlipSprite(false);
                nextFlipTime = Time.time + flipCooldown;
            }
            else if (!isFacingLeft && (transform.position.x > rightPatrol.position.x || collideWithGround || stuck))
            {
                isFacingLeft = true;
                FlipSprite(true);
                nextFlipTime = Time.time + flipCooldown;
            }
        }

        previousFramePosition = transform.position;
    }


    void BobCheck()
    {
        if (isGoingUp && transform.position.y > originalPosition.y + bobDistance)
        {
            isGoingUp = false;
        }
        else if (!isGoingUp && transform.position.y < originalPosition.y - bobDistance)
        {
            isGoingUp = true;
        }
    }

    void FlipSprite(bool faceLeft)
    {
        if (sr != null)
        {
            sr.flipX = !faceLeft;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.CompareTag("Level"))
        {
            collideWithGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.CompareTag("Level"))
        {
            collideWithGround = false;
        }
    }
}
