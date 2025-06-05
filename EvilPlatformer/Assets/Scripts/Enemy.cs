using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public bool isFacingLeft = true;
    public Transform leftPatrol, rightPatrol;
    public Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        PatrolRoute();
        if (isFacingLeft)
        {
            rb.AddForce(Vector2.left*speed*Time.deltaTime);
        }
        else 
        {
            rb.AddForce(Vector2.right*speed*Time.deltaTime);
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
}
