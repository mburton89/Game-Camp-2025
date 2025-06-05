using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    public float boostMultiplier;
    public ParticleSystem ps;
    public int dashSeconds;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Pressed");
            DashParticles();
        }
    }

    void increase()
    {
        ps.startSize *= boostMultiplier;
    }

    void decrease()
    {
        ps.startSize /= boostMultiplier;
    }

    private IEnumerator ParticlesCooroutine()
    {
        increase();
        yield return new WaitForSeconds(dashSeconds);
        decrease();
    }

    public void DashParticles()
    {
        StartCoroutine(ParticlesCooroutine());
    }
}
