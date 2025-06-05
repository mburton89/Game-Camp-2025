using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDestroyer : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Destroyable>())
        {
            collision.gameObject.GetComponent<Destroyable>().Destroy();
        }
    }
}
