using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpHeight;
    public int coyoteFrames;
    public int jumps;

    public float acceleration;
    public float maxSpeed;

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
        PlayerInput();
    }
    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space)) { StartCoroutine(Dash()); }
        if (!is_dashing)
        {
            Left();
            Right();
            Jump();
        }
        
    }
    void Left()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(Vector2.left * acceleration);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    void Right()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(Vector2.right * acceleration);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (tempJumps > 0 && tempFrames > 0)
            {
                tempFrames = 0;

                rb.velocity = new Vector2(rb.velocity.x,jumpHeight);

                tempJumps--;
            }
            else if (tempJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);

                tempJumps--;
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
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.transform.localScale.x * dashSpeed, 0);
            yield return new WaitForSeconds(dashDuration);
            rb.gravityScale = originalGravity;
            is_dashing = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Level")
        {
            tempJumps = jumps;
            tempFrames = coyoteFrames;
            tempDashes = dashes;
        }
        else
        {
            tempFrames--;
        }
    }
}
