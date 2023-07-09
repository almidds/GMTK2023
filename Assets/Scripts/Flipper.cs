using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour{

    private Vector3 lastPosition;
    [SerializeField]private bool whichFlip;
    // Start is called before the first frame update
    void Start(){
        lastPosition = transform.position;
    }

    void Update(){
        var diff = transform.position.x - lastPosition.x;
        if(diff < 0){
            gameObject.GetComponent<SpriteRenderer>().flipX = whichFlip;
        }
        else if(diff > 0){
            gameObject.GetComponent<SpriteRenderer>().flipX = !whichFlip;
        }
        lastPosition = transform.position;
    }
}
