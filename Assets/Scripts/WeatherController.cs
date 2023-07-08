using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    //public float lightningWaitTime;
    private float lightningTime;
    private bool firingLightning;
    public GameObject lightningOverlay;

    // Start is called before the first frame update
    void Start()
    {
        lightningTime = 15;
    }

    // Update is called once per frame
    void Update()
    {
        lightningTime -= 1 * Time.deltaTime;
        if (lightningTime <= 0 && !firingLightning)
        {
            FireLightning();
            float a = Random.Range(0.05f, 0.2f);
            lightningTime = a;
            firingLightning = true;
        } else if (lightningTime <= 0 && firingLightning)
        {
            FireLightning();
            float a = Random.Range(15f, 30f);
            lightningTime = a;
            firingLightning = false;
            if (Random.Range(0, 2) == 1)
            {
                lightningTime = 0.2f;
            }
        }
    }

    void FireLightning()
    {
        {
            Debug.Log("Lightning fired");
            if (!lightningOverlay.active)
                lightningOverlay.SetActive(true);
            else
                lightningOverlay.SetActive(false);
        }
    }
}
