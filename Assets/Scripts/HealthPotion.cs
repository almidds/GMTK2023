using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour{
    void OnTriggerEnter2D(Collider2D a)
    {
        if (a.gameObject.tag == "Player")
        {
            if (a.GetComponent<PlayerController>().health < a.GetComponent<PlayerController>().maxHealth)
            {
                a.GetComponent<PlayerController>().UpdateHealth(-1);
                Destroy(this.gameObject);
            }
        }
    }
}
