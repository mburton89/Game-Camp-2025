using System.Collections;
using UnityEngine;

public class WesleyPlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public CharacterAnimator2D characterAnimator;

    [Header("Jump Settings")]
    public float jumpHeight = 10f;
    public int coyoteFrames = 6;
    public int jumps = 1;

    [Header("Movement Settings")]
    public float acceleration = 50f;
    public float maxSpeed = 8f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public int dashes = 1;
    public float dashDuration = 0.2f;

    bool grounded;
    bool isDashing;

    int tempFrames;
    int tempJumps;
    int tempDashes;

    // Input buffers
    float inputX;
    bool jumpRequest;
    bool dashRequest;

    private void Start()
    {
        tempFrames = coyoteFrames;
        tempJumps = jumps;
        tempDashes = dashes;
    }

    private void Update()
    {
        // 1) Buffer horizontal input
        inputX = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            inputX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            inputX = 1f;

        // 2) Buffer jump press
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            jumpRequest = true;

        // 3) Buffer dash press
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space))
            dashRequest = true;

        // 4) Update animation state every frame
        UpdateAnimationState();

        // Keep the sprite upright
        rb.rotation = 0f;
    }

    private void FixedUpdate()
    {
        // Handle Dash first if requested
        if (dashRequest && !isDashing && tempDashes > 0)
        {
            dashRequest = false;
            StartCoroutine(Dash());
        }

        // If not dashing, apply normal movement and jump
        if (!isDashing)
        {
            HandleHorizontalMovement();
            HandleJump();
            ClampHorizontalSpeed();
        }

        // Coyote‐time and jump reset
        HandleGroundedState();
    }

    private void HandleHorizontalMovement()
    {
        if (inputX < 0f)
        {
            rb.AddForce(Vector2.left * acceleration);
            sr.transform.localScale = new Vector2(-Mathf.Abs(sr.transform.localScale.x), sr.transform.localScale.y);
        }
        else if (inputX > 0f)
        {
            rb.AddForce(Vector2.right * acceleration);
            sr.transform.localScale = new Vector2(Mathf.Abs(sr.transform.localScale.x), sr.transform.localScale.y);
        }
    }

    private void HandleJump()
    {
        if (!jumpRequest)
            return;

        if ((grounded || tempFrames > 0) && tempJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            grounded = false;
            tempFrames = 0;
            tempJumps--;
            characterAnimator.SetState(CharacterAnimator2D.State.Jump);
            SoundManager.Instance.PlaySound("jump");
        }
        else if (tempJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            tempJumps--;
            characterAnimator.SetState(CharacterAnimator2D.State.Jump);
            SoundManager.Instance.PlaySound("jump");
        }

        jumpRequest = false;
    }

    private void ClampHorizontalSpeed()
    {
        if (rb.velocity.x > maxSpeed)
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        else if (rb.velocity.x < -maxSpeed)
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
    }

    private void HandleGroundedState()
    {
        if (grounded)
        {
            tempJumps = jumps;
            tempFrames = coyoteFrames;
            tempDashes = dashes;
        }
        else
        {
            if (tempFrames > 0)
                tempFrames--;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        tempDashes--;
        float originalGravity = rb.gravityScale;
        float originalDrag = rb.drag;

        rb.gravityScale = 0f;
        rb.drag = 0f;

        if (sr.transform.localScale.x > 0)
            rb.velocity = new Vector2(dashSpeed, 0f);
        else
            rb.velocity = new Vector2(-dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        rb.drag = originalDrag;
        isDashing = false;
    }

    private void UpdateAnimationState()
    {
        if (isDashing)
            return;

        if (!grounded)
        {
            characterAnimator.SetState(CharacterAnimator2D.State.Jump);
            return;
        }

        if (inputX < 0f || inputX > 0f)
        {
            characterAnimator.SetState(CharacterAnimator2D.State.Run);
            return;
        }

        characterAnimator.SetState(CharacterAnimator2D.State.Idle);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Level") && Mathf.Approximately(rb.velocity.y, 0f))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }
}
