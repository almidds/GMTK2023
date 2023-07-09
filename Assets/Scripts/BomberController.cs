using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberController : Enemy{
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private GameObject bomb;
    private Transform player;
    [SerializeField] float timeToMoveMax, moveSpeed;
    private float timeToMove;
    private Vector3 target;
    [SerializeField] private Transform shadow;
    [SerializeField] private GameObject circleAnimator;

    private float xMin = -1f, xMax = 18f, yMin = 0.6f, yMax = 19.8f;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeToMove = timeToMoveMax;
        pickNewTarget();
    }

    void Update(){
        Move();
        UpdateZPos();
        Bob();
        this.GetComponent<Animator>().SetBool("Shooting", false);
    }

    void Move(){
        timeToMove -= Time.deltaTime;
        if(timeToMove <= 0){
            timeToMove = timeToMoveMax;
            pickNewTarget();
        }
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
    }

    void Bob(){
        if(Time.timeScale != 0){
            var tempPos = shadow.position;
            transform.position += Vector3.up * 0.001f * Mathf.Sin(Time.time * 5);
            shadow.position = tempPos;
        }
    }

    void UpdateZPos(){
        transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    shadow.position.y/10);
    }

    void pickNewTarget(){
        float radius = Random.Range(1.5f, 2f);
        target = RandomPointOnCircleEdge(radius);
    }

    private Vector3 RandomPointOnCircleEdge(float radius){
        bool goodPoint = false;
        Vector2 vector2;
        do{
            Vector2 playerPos = player.transform.position;
            vector2 = playerPos + Random.insideUnitCircle.normalized * radius;
            if(vector2.x > xMin && vector2.x < xMax && vector2.y > yMin && vector2.y < yMax){
                goodPoint = true;
            }
        }while(goodPoint == false);

        return new Vector3(vector2.x, vector2.y, 0);
    }

    public override void Shoot(){
        if(player != null){
            Vector2 target = player.transform.position;
            Vector3 start = shootPoint.transform.position;
            GameObject bombTemp = Instantiate(bomb, shootPoint.transform.position, transform.rotation);
            bombTemp.GetComponent<BombController>().CallCurve(start, target);
            GameObject circle = Instantiate(circleAnimator, player.transform.position, transform.rotation);
            circle.GetComponent<CircleAnimator>().numPoints = 20;
            circle.GetComponent<CircleAnimator>().radius = 0.52f;
            this.GetComponent<Animator>().SetBool("Shooting", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if(otherTag=="Player"){
            other.gameObject.GetComponent<PlayerController>().UpdateHealth(1);
        }
    }
}
