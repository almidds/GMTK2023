using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour{
    public AnimationCurve curve;
    [SerializeField] private float duration = 1.0f, heightY = 3.0f;
    [SerializeField] private ParticleSystem explode;
    [SerializeField] private GameObject fire;
    private Coroutine _curveCoroutine;
    public void CallCurve(Vector3 start, Vector2 target){
        _curveCoroutine = StartCoroutine(Curve(start, target));
    }
    public IEnumerator Curve(Vector3 start, Vector2 target){
        float timePassed = 0f;
        Vector2 end = target;
        while(timePassed < duration){
            timePassed += Time.deltaTime;

            float linearT = timePassed / duration;
            float heightT = curve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, heightY, heightT);
            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

            yield return null;
        }
        CreateFire();
        Destroy(this.gameObject);
    }

    private void CreateFire(){
        Instantiate(explode, transform.position, transform.rotation);
        Instantiate(fire, transform.position, transform.rotation);
    }
}
