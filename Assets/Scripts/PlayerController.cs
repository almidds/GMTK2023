using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour{

    [SerializeField]
    private float moveSpeed = 5f, activationCooldownMax = 3f, activationRadius = 3f, invincibilityCooldownMax = 1f;
    private float activationCooldown, invincibilityCooldown;
    private bool invincible;

    [SerializeField]
    private GameObject circleAnimator;

    private bool canActivate = true;

    public int maxHealth;
    public int health;
    public Image[] hearts;

    SpriteRenderer sprite;
    Color unchargedColor = new Color(0.7f, 0.7f, 0.7f, 1);
    // Particle system that activates when charge is ready
    [SerializeField]
    GameObject activationTip;

    private DamageFlash damageFlash;

    void Start(){
        sprite = GetComponent<SpriteRenderer>();
        activationCooldown = activationCooldownMax;

        maxHealth = 4;
        health = 4;
        UpdateUI();

        damageFlash = GetComponent<DamageFlash>();
    }

    void Update(){
        Move();
        CheckActivation();
        checkInvincibility();
    }

    void Move(){
        Vector3 move = Vector3.Normalize(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0)) * moveSpeed;
        transform.position += move * Time.deltaTime;
        transform.position = new Vector3(
                            transform.position.x,
                            transform.position.y,
                            transform.position.y/10);
    }

    void CheckActivation(){
        if(Input.GetKeyDown(KeyCode.Space) && canActivate){
            ActivateEnemies();
            canActivate = false;
        }

        if(!canActivate){
            activationCooldown -= Time.deltaTime;
            if(activationCooldown <= 0f){
                damageFlash.flashColor = new Color(0.85f, 0, 0.75f);
                damageFlash.CallDamageFlash();
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

    void checkInvincibility(){
        if(invincibilityCooldown < 0){
            invincible = false;
        }
        else{
            invincibilityCooldown -= Time.deltaTime;
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
        if(!invincible){
            health -= damage;
            UpdateUI();
            if(health <= 0){
                Die();
            }
            damageFlash.flashColor = Color.white;
            damageFlash.CallDamageFlash();

            invincible = true;
            invincibilityCooldown = invincibilityCooldownMax;
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
