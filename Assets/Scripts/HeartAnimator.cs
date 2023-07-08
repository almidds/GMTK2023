using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartAnimator : MonoBehaviour{

    [SerializeField]
    private float flipTimeMax = 0.8f;
    private float flipTime;
    private int flip = 0;

    public bool full = true;

    public Sprite[] fullHeart;
    public Sprite[] emptyHeart;

    void Update(){
        flipTime -= Time.deltaTime;
        if(flipTime <= 0){
            flip = 1 - flip;
            if(full){
                this.gameObject.GetComponent<Image>().sprite = fullHeart[flip];
            }
            else{
                this.gameObject.GetComponent<Image>().sprite = emptyHeart[flip];
            }
            flipTime = flipTimeMax;
        }
    }
}
