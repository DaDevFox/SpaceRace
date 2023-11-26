using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOscillate : MonoBehaviour
{
    public float period;
    public float min;
    public float max;

    private float time = 0f;
    private Image image;

    public float delayTime = 0f;
    private float delayElapsed = 0f; 


    public float fadeInTime = 0f;
    private float fadeInElapsed = 0f;

    private void Start(){
        image = GetComponent<Image>();
    }

    public float Sin(float time){
        return Mathf.Sin(2f * Mathf.PI * time/period) * (max - min)/2 + (max - min)/2 + min;
    }

    void Update()
    {
        if(delayElapsed < delayTime){
            image.color = Color.black;
            delayElapsed += Time.deltaTime;
        }
        else if(fadeInElapsed < fadeInTime){
            image.color = new Color(fadeInElapsed/fadeInTime * (min + (max - min)/2f), fadeInElapsed/fadeInTime * (min + (max - min)/2f), fadeInElapsed/fadeInTime * (min + (max - min)/2f), 1f);
            fadeInElapsed += Time.deltaTime;
        }else{
            time += Time.deltaTime;
            time %= period;
            image.color = ((Color.white * Sin(time)));
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
    }
}
