using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour{
    //public float lightningWaitTime;
    private float lightningTime;
    private bool firingLightning;
    [SerializeField] private GameObject lightningOverlay;

    void Start(){
        lightningTime = 15;
    }

    void Update(){
        lightningTime -= 1 * Time.deltaTime;
        if (lightningTime <= 0 && !firingLightning){
            FireLightning();
            float a = Random.Range(0.05f, 0.2f);
            lightningTime = a;
            firingLightning = true;
        } else if (lightningTime <= 0 && firingLightning){
            FireLightning();
            float a = Random.Range(15f, 30f);
            lightningTime = a;
            firingLightning = false;
            if (Random.Range(0, 2) == 1){
                lightningTime = 0.2f;
            }
        }
    }

    void FireLightning(){
        {
            if (!lightningOverlay.active)
                lightningOverlay.SetActive(true);
            else
                lightningOverlay.SetActive(false);
        }
    }
}
