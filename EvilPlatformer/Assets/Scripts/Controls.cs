using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public float jumpHeight;
    public int coyoteFrames;
    public int jumps;
    bool grounded;

    public float acceleration;
    public float maxSpeed;
    //public float friction;

    public float dashSpeed;
    public int dashes;
    public float dashDuration;


    int tempFrames;
    int tempJumps;
    int tempDashes;
    bool is_dashing;
    // Start is called before the first frame update
    void Start()
    {
        int tempFrames = coyoteFrames;
        int tempJumps = jumps;
        int tempDashes = dashes;
    }

    // Update is called once per frame
    void Update()
    {
        Grounded();
        PlayerInput();
        rb.rotation = 0;
    }
    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space)) { StartCoroutine(Dash()); }
        if (!is_dashing)
        {
            Left();
            Right();
            Jump();
            if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector2(maxSpeed,rb.velocity.y);
            }
            else if (rb.velocity.x < -maxSpeed)
            {
                rb.velocity = new Vector2(-maxSpeed,rb.velocity.y);
            }
            //if (rb.velocity.x > 0)
            //{
            //    rb.velocity = new Vector2(rb.velocity.x - friction, rb.velocity.y);
            //}
            //else if (rb.velocity.x < 0)
            //{
            //    rb.velocity = new Vector2(-rb.velocity.x + friction, rb.velocity.y);
            //}
        }
    }
    void Left()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(Vector2.left * acceleration);
            sr.transform.localScale = new Vector2(-Mathf.Abs(sr.transform.localScale.x), sr.transform.localScale.y);
        }
    }
    void Right()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            sr.transform.localScale = new Vector2(Mathf.Abs(sr.transform.localScale.x), sr.transform.localScale.y);
            rb.AddForce(Vector2.right * acceleration);
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
            }
            else if (tempJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                tempJumps--;
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
            {
                tempFrames--;
            }
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
            {
                rb.velocity = new Vector2(dashSpeed, 0);
            }
            else if (sr.transform.localScale.x < 0)
            {
                rb.velocity = new Vector2(-dashSpeed, 0);
            }
            yield return new WaitForSeconds(dashDuration);
            rb.gravityScale = originalGravity;
            rb.drag = originalDrag;
            is_dashing = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Level" && rb.velocity.y == 0f)
        {
            grounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }
}