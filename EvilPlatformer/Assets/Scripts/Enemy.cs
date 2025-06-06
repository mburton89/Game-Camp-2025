using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float speed, bobDistance, bobForce;
    public bool isFacingLeft = true, isGoingUp = true;
    public Transform leftPatrol, rightPatrol;
    private Vector2 originalPosition, previousFramePosition;
    public Rigidbody2D rb;
    


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        PatrolRoute();
        BobCheck();

        if (isFacingLeft)
        {
            rb.AddForce(Vector2.left*speed*Time.deltaTime);
        }
        else 
        {
            rb.AddForce(Vector2.right*speed*Time.deltaTime);
        }
        if (isGoingUp)
        {
            rb.AddForce(Vector2.up*bobForce*Time.deltaTime);
            Debug.Log("Bob");

        }
        else
        {
            rb.AddForce(Vector2.down*bobForce*Time.deltaTime);
            Debug.Log("Bob");
        }
    }

    void PatrolRoute()
    {
        if (isFacingLeft && gameObject.transform.position.x < leftPatrol.transform.position.x)
        {
            isFacingLeft = false;
        }
        else if (!isFacingLeft && gameObject.transform.position.x > rightPatrol.transform.position.x)
        {
            isFacingLeft = true;
        }


        if (isFacingLeft && previousFramePosition == new Vector2 (gameObject.transform.position.x,gameObject.transform.position.y))
        {
            isFacingLeft = false;
        }
        else if (!isFacingLeft && previousFramePosition == new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y))
        {
            isFacingLeft= true;
        }
    }
    void BobCheck()
    {
        if (isGoingUp && gameObject.transform.position.y > originalPosition.y + bobDistance)
        {
            isGoingUp = false;
        }
        else if (!isGoingUp && gameObject.transform.position.y < originalPosition.y - bobDistance)
        {
            isGoingUp = true;
        }
    }
}
