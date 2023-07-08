using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour{
    [SerializeField] private float lifetime;
    [SerializeField] private ParticleSystem fire, smoke;
    float timer, initialIntensity = 200;
    [SerializeField] private int damage;
    void Start(){
        timer = lifetime;
    }

    void Update(){
        Debug.Log(timer);
        timer -= Time.deltaTime;
        var emission = fire.emission;
        emission.rateOverTime = Mathf.Lerp(initialIntensity, 0, 1 - timer/lifetime);
        emission = smoke.emission;
        emission.rateOverTime = Mathf.Lerp(initialIntensity, 0, 1 - timer/lifetime);
        if(timer <= 0){
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        string otherTag = other.gameObject.tag;
        if(otherTag != "Projectile"){
            switch(otherTag){
                case "Enemy":
                    other.gameObject.GetComponent<Enemy>().UpdateHealth(damage);
                    break;
                case "Player":
                    other.gameObject.GetComponent<PlayerController>().UpdateHealth(damage);
                    break;
            }
        }
    }

}
