using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAnimator : MonoBehaviour{

    [SerializeField]
    private float timeToLive;

    public float radius;
    public int numPoints;
    float step;
    // Start is called before the first frame update
    void Start(){
        DrawCircle();
        float width = this.gameObject.GetComponent<LineRenderer>().startWidth;
        step = width / timeToLive;
    }

    // Update is called once per frame
    void Update(){
        float currentWidth = this.gameObject.GetComponent<LineRenderer>().startWidth;
        this.gameObject.GetComponent<LineRenderer>().startWidth = currentWidth - step * Time.deltaTime;
        this.gameObject.GetComponent<LineRenderer>().endWidth = currentWidth - step * Time.deltaTime;
        timeToLive -= Time.deltaTime;
        if(timeToLive < 0){
            Destroy(this.gameObject);
        }
    }

    void DrawCircle(){
        this.gameObject.GetComponent<LineRenderer>().positionCount = numPoints;
        Vector3[] circleCoords = new Vector3[numPoints];
        int i = 0;
        for(float theta = 0; theta <= 2 * Mathf.PI; theta += 2 * Mathf.PI/numPoints){
            float x = transform.position.x + radius * Mathf.Cos(theta);
            float y = transform.position.y + radius * Mathf.Sin(theta);
            circleCoords[i] = new Vector3(x, y, 0);
            Debug.Log(circleCoords[i]);
            i++;
        }
        this.gameObject.GetComponent<LineRenderer>().SetPositions(circleCoords);
    }
}
