using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour{

    public int health;
    public GameObject projectile;
    private DamageFlash damageFlash;

    [SerializeField]private GameObject healthPotion;
    [SerializeField]private GameObject deathExplosion;

    private void Awake(){
        damageFlash = GetComponent<DamageFlash>();
    }

    // Update is called once per frame
    public void UpdateHealth(int damage){
        health -= damage;
        if(health <= 0){
            Die();
        }
        damageFlash.CallDamageFlash();
    }

    public abstract void Shoot();

    public void Die(){
        if (Random.Range(0, 20) == 1){
            Instantiate(healthPotion, transform.position + new Vector3(0, 0, 1), transform.rotation);
        }
        Instantiate(deathExplosion, transform.position, Quaternion.Euler(90, 0, 0));
        Destroy(this.gameObject);
    }
}