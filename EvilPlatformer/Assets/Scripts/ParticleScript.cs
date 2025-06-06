using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    public float boostMultiplier, maxSize;
    public ParticleSystem ps;
    public int dashSeconds;

    void start()
    {
        maxSize = ps.startSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ps.startSize <= maxSize)
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
