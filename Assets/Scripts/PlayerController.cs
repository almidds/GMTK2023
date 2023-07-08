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

    void Start(){
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
