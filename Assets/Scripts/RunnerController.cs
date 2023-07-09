using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController : Enemy{
    private Transform player;
    [SerializeField] private float moveSpeed, waitTimeMax;
    private float waitTime;

    [SerializeField] private Transform shadow;
    [SerializeField] private Transform trailChild;

    private bool move;
    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update(){
        if(waitTime > 0){
            waitTime -= Time.deltaTime;
        }
        else{
            if(move == false){
                move = true;
            }
        }

        if(move && player != null){
            Move();
        }
        Bob();
        trailChild.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+0.5f);
    }

    private void Move(){
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.y/10);
    }

    void Bob(){
        if(Time.timeScale != 0){
            var tempPos = shadow.position;
            transform.position += Vector3.up * 0.001f * Mathf.Sin(Time.time * 5);
            shadow.position = tempPos;
        }
    }

    public override void Shoot(){
        move = false;
        waitTime = waitTimeMax;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if(otherTag=="Player"){
            other.gameObject.GetComponent<PlayerController>().UpdateHealth(1);
            UpdateHealth(10);
        }
    }
}
