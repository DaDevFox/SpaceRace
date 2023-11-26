using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GroupedFade : MonoBehaviour
{
    public float delay = 0f;
    public float time = 1f;
    private bool alpha = true;
    public bool inverse;
    public bool setOnStart = true;

    public GameObject[] fadeObjects;


    
    void Start()
    {
        StartCoroutine("Fade");
    }

    void OnEnable() => Start();

    public IEnumerator Fade(){
        if(setOnStart)
            SetFade(inverse ? 1f : 0f);
        
        yield return new WaitForSecondsRealtime(delay);

        float elapsed = 0f;
        while(elapsed < time){
            SetFade(inverse ? 1f - elapsed/time : elapsed/time);
            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
        }

        SetFade(inverse ? 0f : 1f);
    }

    public void SetFade(float val){
        foreach(GameObject obj in fadeObjects){
            Image image = obj.GetComponent<Image>();
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            if(alpha){
                if(image)
                    image.color = new Color(image.color.r, image.color.g, image.color.b, val);
                if(text)
                    text.color = new Color(text.color.r, text.color.g, text.color.b, val);
            }
        }
    }
}
