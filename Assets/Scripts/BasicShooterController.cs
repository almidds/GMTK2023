using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooterController : Enemy{
    [SerializeField]
    private Transform shootPoint;
    private Transform player;

    private float angle;
    private float startAngle;

    [SerializeField]
    private float moveSpeed, angularSpeed;
    private float randomMoveSpeed, randomAngularSpeed;
    private float rotationDirection;

    private float moveTime;
    private bool isRotating;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update(){
        Move();
    }

    public override void Shoot(){
        if(player != null){
            Vector3 direction = player.position - transform.position;
            GameObject bullet = Instantiate(projectile, shootPoint.position, transform.rotation);
            bullet.GetComponent<ProjectileController>().direction = Vector3.Normalize(direction);
            bullet.GetComponent<ProjectileController>().parentName = gameObject.name;
        }
    }

    private void Move(){
        moveTime -= Time.deltaTime;
        if(moveTime <= 0){
            isRotating = !isRotating;
            moveTime = Random.Range(2f, 5f);
            randomMoveSpeed = moveSpeed * Random.Range(0.9f, 1.1f);
            randomAngularSpeed = angularSpeed * Random.Range(0.9f, 1.1f);
            rotationDirection = Mathf.Sign(Random.Range(-1f, 1f));
        }
        if(isRotating){
            orbitPlayer();
        }
        else{
            moveTowardsPlayer();
        }
    }

    private void orbitPlayer(){
        Quaternion rotation = transform.rotation;
        transform.RotateAround(player.position, Vector3.forward, rotationDirection * angularSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }

    private void moveTowardsPlayer(){
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * moveSpeed);
    }
}
