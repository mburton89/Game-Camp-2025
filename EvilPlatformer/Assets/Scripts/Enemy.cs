using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float speed, bobDistance, bobForce;
    public bool isFacingLeft = true, isGoingUp = true, collideWithGround;
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

        }
        else
        {
            rb.AddForce(Vector2.down*bobForce*Time.deltaTime);
        }
    }

    void PatrolRoute()
    {
        if (isFacingLeft && (gameObject.transform.position.x < leftPatrol.transform.position.x || collideWithGround))
        {
            isFacingLeft = false;
        }
        else if (!isFacingLeft && (gameObject.transform.position.x > rightPatrol.transform.position.x || collideWithGround))
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            collideWithGround = true;
            Debug.Log(collideWithGround);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collideWithGround = false;
        if (isFacingLeft)
        {
            if (gameObject.GetComponent<SpriteRenderer>().flipX)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else
        {
            if (!gameObject.GetComponent<SpriteRenderer>().flipX)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
}
