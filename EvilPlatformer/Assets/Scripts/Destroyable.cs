using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public GameObject explosionPrefab;
    public void Destroy()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation, null);

        if (gameObject.tag == "Player")
        {
            GameManager.instance.GameOver();
            SoundManager.Instance.PlaySound("damage");
        }

        Destroy(gameObject);
    }
}
