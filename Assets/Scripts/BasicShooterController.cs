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
    private int movementIndex;
    [SerializeField] private Transform shadow;

    delegate void movementMethod();
    List<movementMethod> movementMethods = new List<movementMethod>();

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        movementMethods.Add(orbitPlayer);
        movementMethods.Add(moveTowardsPlayer);
        movementMethods.Add(wait);
    }

    void Update(){
        Move();
        Bob();
    }

    void Bob(){
        var tempPos = shadow.position;
        transform.position += Vector3.up * 0.0005f * Mathf.Sin(Time.time * 5);
        shadow.position = tempPos;
    }

    public override void Shoot(){
        if(player != null){
            Vector2 direction = player.position - transform.position;
            GameObject bullet = Instantiate(projectile, shootPoint.position, transform.rotation);
            bullet.GetComponent<ProjectileController>().direction = Vector3.Normalize(new Vector3(direction.x, direction.y, 0));
            bullet.GetComponent<ProjectileController>().parentName = gameObject.name;
        }
    }

    private void Move(){
        moveTime -= Time.deltaTime;
        if(moveTime <= 0){
            movementIndex = Random.Range(0, 3);
            if(movementIndex==2){
                moveTime = Random.Range(1f, 2f);
            }
            else{
                moveTime = Random.Range(2f, 5f);
            }
            randomMoveSpeed = moveSpeed * Random.Range(0.9f, 1.1f);
            randomAngularSpeed = angularSpeed * Random.Range(0.9f, 1.1f);
            rotationDirection = Mathf.Sign(Random.Range(-1f, 1f));
        }
        movementMethods[movementIndex]();
    }

    private void orbitPlayer(){
        Quaternion rotation = transform.rotation;
        transform.RotateAround(player.position, Vector3.forward, rotationDirection * angularSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }

    private void moveTowardsPlayer(){
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * moveSpeed);
    }

    private void wait(){
        transform.position = transform.position;
    }
}
