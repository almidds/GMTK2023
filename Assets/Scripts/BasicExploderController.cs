using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicExploderController : Enemy
{
    private Transform player;
    [SerializeField]
    private float moveSpeed;
    private float randomMoveSpeed;

    private float moveTime;

    private bool waiting;
    private bool exploding;

    public float explosionRange;
    public float explosionTime;
    public GameObject explosionParticles;

    [SerializeField]
    private GameObject circleAnimator;
    [SerializeField] private Transform shadow;
    [SerializeField] private GameObject fuse;

    private Coroutine _explodeCoroutine;
    private bool stop = false;

    private void CallExplode(){
        _explodeCoroutine = StartCoroutine(Explode());
    }

    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update(){
        if (!exploding){
            Move();
            Bob();
        }
        else{
            if(stop == false){
                CallExplode();
                stop = true;
            }
        }
    }

    void Bob(){
        if(Time.timeScale != 0){
            var tempPos = shadow.position;
            transform.position += Vector3.up * 0.001f * Mathf.Sin(Time.time * 5);
            shadow.position = tempPos;
        }
    }

    private void MoveTowardsPlayer(){
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * randomMoveSpeed);
        transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.y/10);
    }

    private void Wait(){
        transform.position = transform.position;
    }

    public override void Shoot(){
        exploding = true;
    }

    public IEnumerator Explode(){
        float timePassed = 0f;
        float finalScale = 0.95f;
        Color finalColor = Color.red;
        fuse.SetActive(true);
        while(timePassed < explosionTime){
            timePassed += Time.deltaTime;
            this.transform.localScale = new Vector3(
                                                transform.localScale.x,
                                                Mathf.Lerp(1, finalScale, timePassed/explosionTime),
                                                transform.localScale.z);
            this.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, finalColor, timePassed/explosionTime);
            yield return null;
        }
        GameObject circle = Instantiate(circleAnimator, transform.position, transform.rotation);
        circle.GetComponent<CircleAnimator>().numPoints = 20;
        circle.GetComponent<CircleAnimator>().radius = explosionRange;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        Debug.Log("A");
        foreach (var enemy in enemies){
            if (enemy.gameObject.tag == "Enemy"){
                if(enemy.gameObject.name != this.name){
                    enemy.GetComponent<Enemy>().UpdateHealth(3);
                }
            }
        }
        foreach (var GameObject in enemies){
            if (GameObject.tag == "Player"){
                player.GetComponent<PlayerController>().UpdateHealth(3);
            }
        }
        Instantiate(explosionParticles, transform.position + new Vector3(0, 0, 1), transform.rotation);
        Destroy(this.gameObject);
    }

    private void Move(){
        moveTime -= Time.deltaTime;

        if (moveTime <= 0){
            if (Random.Range(0, 4) == 1)
                waiting = true;
            else
                waiting = false;
            moveTime = Random.Range(1f, 4f);
        }
        if (waiting){
            Wait();
        }
        else{
            randomMoveSpeed = moveSpeed * Random.Range(0.9f, 1.1f);
            MoveTowardsPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if(otherTag=="Player"){
            exploding = true;
        }
    }
}