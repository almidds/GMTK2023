using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour{
    [SerializeField]
    private float timeToLive = 10f;

    [SerializeField]
    private float moveSpeed = 1.5f;

    [SerializeField]
    private int damage = 5;

    public string parentName;
    public Vector3 direction;

    void Update(){
        transform.position += direction * moveSpeed * Time.deltaTime;
        timeToLive -= Time.deltaTime;
        if(timeToLive < 0){
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if(other.gameObject.name != parentName && otherTag != "Projectile"){
            switch(otherTag){
                case "Enemy":
                    other.gameObject.GetComponent<Enemy>().UpdateHealth(damage);
                    break;
                case "Player":
                    other.gameObject.GetComponent<PlayerController>().UpdateHealth(damage);
                    break;
            }
            Explode();
        }
    }

    private void Explode(){
        Destroy(this.gameObject);
    }

}
