using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour{

    [SerializeField]
    private float moveSpeed = 5f, activationCooldownMax = 3f, activationRadius = 3f;

    [SerializeField]
    private GameObject circleAnimator;

    private float activationCooldown;
    private bool canActivate = true;

    public int maxHealth;
    public int health;
    public Image[] hearts;

    SpriteRenderer sprite;
    Color unchargedColor = new Color(0.7f, 0.7f, 0.7f, 1);
    // Particle system that activates when charge is ready
    [SerializeField]
    GameObject activationTip;

    void Start(){
        sprite = GetComponent<SpriteRenderer>();
        activationCooldown = activationCooldownMax;

        maxHealth = 4;
        health = 4;
        UpdateUI();
    }

    void Update(){
        Move();
        checkActivation();
    }

    void Move(){
        Vector3 move = Vector3.Normalize(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0)) * moveSpeed;
        transform.position += move * Time.deltaTime;
        transform.position = new Vector3(
                            transform.position.x,
                            transform.position.y,
                            transform.position.y/10);
    }

    void checkActivation(){
        if(Input.GetKeyDown(KeyCode.Space) && canActivate){
            ActivateEnemies();
            canActivate = false;
        }

        if(!canActivate){
            activationCooldown -= Time.deltaTime;
            if(activationCooldown <= 0f){
                canActivate = true;
                activationCooldown = activationCooldownMax;
                GameObject particles = Instantiate(activationTip, transform.position + new Vector3(0,0,1), transform.rotation);
                particles.transform.SetParent(transform);
            }
            else{
                sprite.color = Color.Lerp(unchargedColor, Color.white, 1-activationCooldown/activationCooldownMax);
            }
        }
    }

    void ActivateEnemies(){
        GameObject circle = Instantiate(circleAnimator, transform.position, transform.rotation);
        circle.GetComponent<CircleAnimator>().numPoints = 20;
        circle.GetComponent<CircleAnimator>().radius = activationRadius;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, activationRadius);
        foreach(var enemy in enemies){
            if(enemy.gameObject.tag == "Enemy"){
                enemy.GetComponent<Enemy>().Shoot();
            }
        }
    }

    public void UpdateHealth(int damage){
        Debug.Log("Ow oof oowie");
        health -= damage;
        UpdateUI();
        if(health <= 0){
            Die();
        }
    }

    void UpdateUI(){
        for(int i = 0; i < hearts.Length; i++){
            if(i < health){
                hearts[i].gameObject.GetComponent<HeartAnimator>().full = true;
            }
            else{
                hearts[i].gameObject.GetComponent<HeartAnimator>().full = false;
            }

            if(i < maxHealth){
                hearts[i].enabled = true;
            }
            else{
                hearts[i].enabled = false;
            }
        }
    }

    private void Die(){
        Destroy(this.gameObject);
    }
}
