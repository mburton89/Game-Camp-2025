using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpHeight;
    public float moveSpeed;
    public float dashSpeed;
    public int coyoteFrames;
    int tempFrames = coyoteFrames;
    public int jumps = 2;
    public int dashes = 1;
    int tempJumps = jumps;
    int tempDashes = dashes;
    bool is_dashing = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(Vector2 * friction);
        Left();
        Right();
        Jump();
    }

    void Left()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(Vector2.left * moveSpeed);
        }
    }
    void Right()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(Vector2.right * moveSpeed);
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (tempJumps > 0 && tempFrames > 0)
            {
                tempFrames = 0;
                rb.velocity = Vector2.up * jumpHeight;
                tempJumps--;
            } 
            else if (tempJumps > 0) 
            {
                rb.velocity = Vector2.up * jumpHeight;
                tempJumps--;
            }
            else{}
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            tempJumps = jumps;
            tempFrames = coyoteFrames;
        }
        else
        {
            tempFrames--;
        }
    }
}
