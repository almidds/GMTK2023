using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    [SerializeField]
    private float moveSpeed = 5f, activationCooldownMax = 3f, activationRadius = 3f;

    [SerializeField]
    private GameObject circleAnimator;

    private float activationCooldown;
    private bool canActivate = true;

    void Start(){
        activationCooldown = activationCooldownMax;
    }

    void Update(){
        Vector3 move = Vector3.Normalize(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0)) * moveSpeed;
        transform.position += move * Time.deltaTime;

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
        Debug.Log("I have taken damage");
    }
}
