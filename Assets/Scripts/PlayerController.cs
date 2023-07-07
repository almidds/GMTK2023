using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    [SerializeField]
    private float moveSpeed = 5f, activationCooldownMax = 3f;

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
                Debug.Log("Cooldown is ready");
            }
        }

    }

    void ActivateEnemies(){
        Debug.Log("I have activated");
    }
}
