using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float speed, bobDistance, bobForce, leftPatrolBoundary, rightPatrolBoundary;
    public bool isFacingLeft = true, isGoingUp = true;
    public Transform leftPatrol, rightPatrol;
    private Vector2 originalPosition;
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
        }
        else
        {
            rb.AddForce(Vector2.down*bobForce*Time.deltaTime);
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
    }
    void BobCheck()
    {
        if (isGoingUp && gameObject.transform.position.y > originalPosition.y + bobDistance)
        {
            isGoingUp = false;
            Debug.Log("Going Down");
        }
        else if (!isGoingUp && gameObject.transform.position.y < originalPosition.y - bobDistance)
        {
            isGoingUp = true;
        }
    }
}
