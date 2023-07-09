using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour{

    [ColorUsage(true, true)]
    public Color flashColor = Color.white;
    [SerializeField]
    private float flashTime = 0.25f;
    [SerializeField]
    private AnimationCurve flashCurve;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private Coroutine _damageFlashCoroutine;
    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    public void CallDamageFlash(){
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher(){
        SetFlashColor();
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while(elapsedTime < flashTime){
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, flashCurve.Evaluate(elapsedTime), (elapsedTime / flashTime));
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }

    private void SetFlashColor(){
        material.SetColor("_FlashColour", flashColor);
    }

    private void SetFlashAmount(float amount){
        material.SetFloat("_FlashAmount", amount);
    }
}
