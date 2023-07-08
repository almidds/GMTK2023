using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour{

    public int health;
    public GameObject projectile;
    private DamageFlash damageFlash;

    private void Awake(){
        damageFlash = GetComponent<DamageFlash>();
    }

    // Update is called once per frame
    public void UpdateHealth(int damage){
        Debug.Log("I have taken damage");
        health -= damage;
        if(health <= 0){
            Die();
        }
        damageFlash.CallDamageFlash();
    }

    public abstract void Shoot();

    void Die(){
        Debug.Log("I have died");
        Destroy(this.gameObject);
    }
}
