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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!exploding)
            Move();
        else
            explosionTime -= Time.deltaTime;
        if (explosionTime <= 0)
            Explode();
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * randomMoveSpeed);
    }

    private void Wait()
    {
        transform.position = transform.position;
    }

    public override void Shoot()
    {
        exploding = true;
    }

    public void Explode()
    {
        GameObject circle = Instantiate(circleAnimator, transform.position, transform.rotation);
        circle.GetComponent<CircleAnimator>().numPoints = 20;
        circle.GetComponent<CircleAnimator>().radius = explosionRange;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.tag == "Enemy")
            {
                enemy.GetComponent<Enemy>().UpdateHealth(3);
            }
        }
        foreach (var GameObject in enemies)
        {
            if (GameObject.tag == "Player")
            {
                player.GetComponent<PlayerController>().UpdateHealth(3);
            }
        }
        Instantiate(explosionParticles, transform.position + new Vector3(0, 0, 1), transform.rotation);
        //particles.transform.SetParent(transform);
        Destroy(this.gameObject);
    }

    private void Move()
    {
        moveTime -= Time.deltaTime;

        if (moveTime <= 0)
        {
            if (Random.Range(0, 4) == 1)
                waiting = true;
            else
                waiting = false;
            moveTime = Random.Range(1f, 4f);
        }
        if (waiting)
        {
            Wait();
        }
        else
        {
            randomMoveSpeed = moveSpeed * Random.Range(0.9f, 1.1f);
            MoveTowardsPlayer();
        }
    }
}