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
    float upperX, lowerX;
    int ghostIndex = 0;

    private float xMin = -1f, xMax = 18f, yMin = 0.6f, yMax = 19.8f;

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
            "{0:0}:{1:00}",
            Mathf.FloorToInt(timer/60),
            Mathf.FloorToInt(timer % 60));
    }

    private void UpdateCameraBounds(){
        lowerX = _camera.ViewportToWorldPoint(new Vector3(0,0,_camera.nearClipPlane)).x;
        upperX = _camera.ViewportToWorldPoint(new Vector3(1,1,_camera.nearClipPlane)).x;
    }

    private void CheckSpawner(){
        if(timeBetweenSpawns < 0){
            SpawnEnemy();
            timeBetweenSpawns = timeBetweenSpawnsMax;
        }
        else{
            timeBetweenSpawns -= Time.deltaTime;
        }
    }

    private void SpawnEnemy(){
        GameObject enemyToSpawn = enemies[Random.Range(0, maxIndex+1)];
        enemyToSpawn.name = "Ghost " + ghostIndex.ToString();
        ghostIndex++;
        Vector3 spawnPoint =  RandomPointOnCircleEdge(spawnRadius + (upperX - lowerX)/2);
        Instantiate(enemyToSpawn, spawnPoint, transform.rotation);
    }

    private Vector3 RandomPointOnCircleEdge(float radius){
        bool goodPoint = false;
        Vector2 vector2;
        do{
            Vector2 cameraOffset = new Vector2(_camera.transform.position.x, _camera.transform.position.y);
            vector2 = cameraOffset + Random.insideUnitCircle.normalized * radius;
            if(vector2.x > xMin && vector2.x < xMax && vector2.y > yMin && vector2.y < yMax){
                goodPoint = true;
            }
        }while(goodPoint == false);

        return new Vector3(vector2.x, vector2.y, 0);
    }
}
