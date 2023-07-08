using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour{
    // Amount of time the game goes on for, how long between waves
    [SerializeField] private float timer = 300f, timeBetweenWavesMax = 40f, spawnRadius, timeBetweenSpawnsMax;
    private float timeBetweenWaves, timeBetweenSpawns;
    [SerializeField] private TextMeshProUGUI timerText, winLoseText;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject[] enemies;
    private int maxIndex = 0;

    Vector3 bottomCorner, topCorner;
    float upperX, lowerX, upperY, lowerY;

    void Start(){
        timeBetweenSpawns = timeBetweenSpawnsMax;
    }

    void Update(){
        timer -= Time.deltaTime;
        UpdateTimer();
        UpdateCameraBounds();
        CheckSpawner();
    }

    private void UpdateTimer(){
        timerText.SetText(
            "{0:0}:{1:0}",
            Mathf.FloorToInt(timer/60),
            Mathf.FloorToInt(timer % 60));
    }

    private void UpdateCameraBounds(){
        bottomCorner = _camera.ViewportToWorldPoint(new Vector3(0,0,_camera.nearClipPlane));
        lowerX = bottomCorner.x;
        lowerY = bottomCorner.y;
        topCorner = _camera.ViewportToWorldPoint(new Vector3(1,1,_camera.nearClipPlane));
        upperX = topCorner.x;
        upperY = topCorner.y;
    }

    private void CheckSpawner(){
        if(timeBetweenSpawns < 0){
            Debug.Log("Spawning an enemy");
            SpawnEnemy();
            timeBetweenSpawns = timeBetweenSpawnsMax;
        }
        else{
            timeBetweenSpawns -= Time.deltaTime;
        }
    }

    private void SpawnEnemy(){
        GameObject enemyToSpawn = enemies[Random.Range(0, maxIndex+1)];
        bool goodSpawn = false;
        float xPoint = 0, yPoint = 0;
        while(!goodSpawn){
            xPoint = Random.Range(lowerX - spawnRadius, upperX + spawnRadius);
            yPoint = Random.Range(lowerY - spawnRadius, upperY + spawnRadius);
            if(xPoint > upperX || xPoint < lowerX){
                if(yPoint > upperY || yPoint < lowerY){
                    goodSpawn = true;
                }
            }
        }
        Instantiate(enemyToSpawn, new Vector3(xPoint, yPoint, 0), transform.rotation);
    }
}
