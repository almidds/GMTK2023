using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour{
    [SerializeField] private Image black;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip bell;
    private bool activated = false;

    void Update(){
        if(Input.GetKeyDown(KeyCode.Return) && !activated){
            AudioSource.PlayClipAtPoint(bell, transform.position);
            activated = true;
            StartCoroutine(Fading());
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            Application.Quit();
        }
    }

    IEnumerator Fading(){
        anim.SetBool("Fade", true);
        yield return new WaitUntil(()=>black.color.a==1);
        SceneManager.LoadScene("SampleScene");
    }
}
