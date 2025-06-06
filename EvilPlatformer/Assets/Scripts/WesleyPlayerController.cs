using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WesleyPlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public float jumpHeight;
    public int coyoteFrames;
    public int jumps;
    bool grounded;

    public float acceleration;
    public float maxSpeed;

    public float dashSpeed;
    public int dashes;
    public float dashDuration;

    int tempFrames;
    int tempJumps;
    int tempDashes;
    bool is_dashing;

    public CharacterAnimator2D characterAnimator;

    void Start()
    {
        // Ensure our temporary counters start out equal to their base values:
        tempFrames = coyoteFrames;
        tempJumps = jumps;
        tempDashes = dashes;
    }

    void Update()
    {
        Grounded();
        PlayerInput();

        // After processing input/movement, decide which animation to play:
        UpdateAnimationState();

        rb.rotation = 0;
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }

        if (!is_dashing)
        {
            Left();
            Right();
            Jump();

            // Clamp horizontal speed:
            if (rb.velocity.x > maxSpeed)
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            else if (rb.velocity.x < -maxSpeed)
                rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
    }

    void Left()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(Vector2.left * acceleration);
            sr.transform.localScale = new Vector2(-Mathf.Abs(sr.transform.localScale.x), sr.transform.localScale.y);
            // We’ll let UpdateAnimationState() handle setting Run versus Idle
        }
    }

    void Right()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(Vector2.right * acceleration);
            sr.transform.localScale = new Vector2(Mathf.Abs(sr.transform.localScale.x), sr.transform.localScale.y);
            // We’ll let UpdateAnimationState() handle setting Run versus Idle
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if ((grounded || tempFrames > 0) && tempJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                grounded = false;
                tempFrames = 0;
                tempJumps--;
                SoundManager.Instance.PlaySound("jump");

                // Immediately switch to Jump animation:
                characterAnimator.SetState(CharacterAnimator2D.State.Jump);
            }
            else if (tempJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                tempJumps--;
                SoundManager.Instance.PlaySound("jump");
                characterAnimator.SetState(CharacterAnimator2D.State.Jump);
            }
        }
    }

    void Grounded()
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

    IEnumerator Dash()
    {
        if (tempDashes > 0)
        {
            is_dashing = true;
            tempDashes--;
            float originalGravity = rb.gravityScale;
            float originalDrag = rb.drag;
            rb.gravityScale = 0;
            rb.drag = 0;

            if (sr.transform.localScale.x > 0)
                rb.velocity = new Vector2(dashSpeed, 0);
            else
                rb.velocity = new Vector2(-dashSpeed, 0);

            yield return new WaitForSeconds(dashDuration);
            rb.gravityScale = originalGravity;
            rb.drag = originalDrag;
            is_dashing = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Level" && Mathf.Approximately(rb.velocity.y, 0f))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }

    private void UpdateAnimationState()
    {
        // 1) If we’re dashing, do not override the animation here
        if (is_dashing)
            return;

        // 2) If we’re in the air (not grounded), show Jump
        if (!grounded)
        {
            characterAnimator.SetState(CharacterAnimator2D.State.Jump);
            return;
        }

        // 3) If we’re on the ground but have horizontal input, show Run
        bool pressingLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool pressingRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        if (pressingLeft || pressingRight)
        {
            characterAnimator.SetState(CharacterAnimator2D.State.Run);
            return;
        }

        // 4) Otherwise (grounded + no horizontal input), show Idle
        characterAnimator.SetState(CharacterAnimator2D.State.Idle);
    }
}
