using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public RigidBody2D rb;
    public float jumpHeight;
    public int coyoteFrames;
    public int jumps;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Left()
    {
        if (Input.GetKeyDown())
        {

        }
    }
    void Right()
    {
        if (Input.GetKeyDown())
        {

        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (jumps > 0)
            {
                coyoteFrames = 0;
                rb.velocity = Vector2.up * jumpHeight;
                jumps--;
            }
        }
    }
}
