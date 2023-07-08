using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour{
    [SerializeField] private float lifetime;
    [SerializeField] private ParticleSystem fire, smoke;
    float timer, initialIntensity = 200;
    [SerializeField] private int damage;
    private float hurtTimeMax = 0.5f, hurtTime = -0.1f;
    void Start(){
        timer = lifetime;
    }

    void Update(){
        timer -= Time.deltaTime;
        var emission = fire.emission;
        emission.rateOverTime = Mathf.Lerp(initialIntensity, 0, 1 - timer/lifetime);
        emission = smoke.emission;
        emission.rateOverTime = Mathf.Lerp(initialIntensity, 0, 1 - timer/lifetime);
        if(timer <= 0){
            Destroy(this.gameObject);
        }
        if(hurtTime > 0){
            hurtTime -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        string otherTag = other.gameObject.tag;
        if(otherTag != "Projectile" && hurtTime < 0){
            switch(otherTag){
                case "Enemy":
                    other.gameObject.GetComponent<Enemy>().UpdateHealth(damage);
                    break;
                case "Player":
                    other.gameObject.GetComponent<PlayerController>().UpdateHealth(damage);
                    break;
            }
            hurtTime = hurtTimeMax;
        }
    }

}
