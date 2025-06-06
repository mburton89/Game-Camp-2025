using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float speed, bobDistance, bobForce;
    public bool isFacingLeft = true, isGoingUp = true, collideWithGround;
    public Transform leftPatrol, rightPatrol;
    private Vector2 originalPosition, previousFramePosition;
    public Rigidbody2D rb;
    private SpriteRenderer sr;

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
        if (isFacingLeft && (transform.position.x < leftPatrol.position.x || collideWithGround))
        {
            isFacingLeft = false;
            FlipSprite(false);
        }
        else if (!isFacingLeft && (transform.position.x > rightPatrol.position.x || collideWithGround))
        {
            isFacingLeft = true;
            FlipSprite(true);
        }

        if (isFacingLeft && previousFramePosition == (Vector2)transform.position)
        {
            isFacingLeft = false;
            FlipSprite(false);
        }
        else if (!isFacingLeft && previousFramePosition == (Vector2)transform.position)
        {
            isFacingLeft = true;
            FlipSprite(true);
        }
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
