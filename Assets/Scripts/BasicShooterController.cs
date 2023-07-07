using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooterController : Enemy{
    [SerializeField]
    private Transform shootPoint;
    private Transform player;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update(){
        
    }

    public override void Shoot(){
        if(player != null){
            Vector3 direction = player.position - transform.position;
            GameObject bullet = Instantiate(projectile, shootPoint.position, transform.rotation);
            bullet.GetComponent<ProjectileController>().direction = Vector3.Normalize(direction);
            bullet.GetComponent<ProjectileController>().parentName = gameObject.name;
        }
    }
}
