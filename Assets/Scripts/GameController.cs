using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour{
    // Amount of time the game goes on for, how long between waves
    [SerializeField] private float timer = 5f, timeBetweenWavesMax = 40f, spawnRadius, timeBetweenSpawnsMax;
    private float timeBetweenWaves, timeBetweenSpawns;
    [SerializeField] private TextMeshProUGUI timerText, winLoseText, winLoseSubText;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject winLoseScreen;
    private bool winLose = false;
    private int maxIndex = 0;
    float upperX, lowerX;
    int ghostIndex = 0;

    private float xMin = -1f, xMax = 18f, yMin = 0.6f, yMax = 19.8f;
    [SerializeField] private GameObject pauseScreen;
    private bool paused = false;

    private Coroutine _destroyCoroutine;

    void CallDestroy(){
        _destroyCoroutine = StartCoroutine(DestroyEnemies());
    }

    void Start(){
        Time.timeScale = 1f;
        timeBetweenSpawns = timeBetweenSpawnsMax;
        timeBetweenWaves = timeBetweenWavesMax;
    }

    void Update(){
        timer -= Time.deltaTime;
        UpdateTimer();
        UpdateCameraBounds();
        CheckSpawner();
        CheckWaves();
        if(Input.GetKeyDown(KeyCode.Escape) && winLose != true){
            PauseGame();
        }
        if(player == null){
            LoseScreen();
        }
        if(timer <= 0){
            WinScreen();
        }
        if(Input.GetKeyDown(KeyCode.Q) && paused){
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.R) && winLose){
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void CheckWaves(){
        timeBetweenWaves -= Time.deltaTime;
        if(timeBetweenWaves < 0){
            maxIndex = Mathf.Min(maxIndex + 1, enemies.Length - 1);
            timeBetweenWaves = timeBetweenWavesMax;
        }
    }

    private void PauseGame(){
        if(paused){
            gameObject.GetComponent<AudioSource>().volume = 0.14f;
            paused = false;
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }
        else{
            gameObject.GetComponent<AudioSource>().volume = 0.01f;
            paused = true;
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }
    }

    private void LoseScreen(){
        gameObject.GetComponent<AudioSource>().volume = 0.01f;
        Time.timeScale = 0;
        winLose = true;
        winLoseText.color = Color.red;
        winLoseText.SetText("You have died");
        winLoseSubText.SetText("Press R to retry");
        winLoseScreen.SetActive(true);
        winLoseText.gameObject.SetActive(true);
        winLoseSubText.gameObject.SetActive(true);
    }

    private void WinScreen(){
        gameObject.GetComponent<AudioSource>().volume = 0.01f;
        Time.timeScale = 0;
        winLose = true;
        winLoseText.SetText("You survived");
        winLoseSubText.SetText("Congratulations! \n Press R to play again");
        winLoseScreen.SetActive(true);
        winLoseText.gameObject.SetActive(true);
        winLoseSubText.gameObject.SetActive(true);
        CallDestroy();
    }

    public IEnumerator DestroyEnemies(){
        float timeBetweenKills = 0.1f;
        float timePassed = 0f;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++){
            enemies[i].GetComponent<Enemy>().Die();
            timePassed = timeBetweenKills;
            while(timePassed > 0){
                timePassed -= Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

    private void UpdateTimer(){
        if(timer > 0){
            timerText.SetText(
                "{0:0}:{1:00}",
                Mathf.FloorToInt(timer/60),
                Mathf.FloorToInt(timer % 60));
        }
        else{
            timerText.SetText("00:00");
        }

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
