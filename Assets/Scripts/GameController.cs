using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour{
    // Amount of time the game goes on for, how long between waves
    [SerializeField]
    private float timer = 300f, timeBetweenWavesMax = 40f, timeBetweenWaves;

    [SerializeField]
    TextMeshProUGUI timerText;

    void Start(){
        
    }

    void Update(){
        timer -= Time.deltaTime;
        UpdateTimer();
    }

    private void UpdateTimer(){
        timerText.SetText(
            "{0:0}:{1:0}",
            Mathf.FloorToInt(timer/60),
            Mathf.FloorToInt(timer % 60));
    }
}
