using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image bgImage;
    public Image imageFill;

    public HealthObj target;

    public Vector3 screenOffset = new Vector3(0f, 10f, 0f);

    public float showTimer;
    public static float showTime = 3f;

    public Color bgImageColor = new Color(159, 159, 159, 255);



    void Start()
    {
        imageFill.fillAmount = 1f;
        target.onHealthChange += HealthChange;
        showTimer = showTime + UnityEngine.Random.Range(-1f, 1f);
    }

    void HealthChange()
    {
        showTimer = showTime;
    }


    void Update()
    {
        transform.position = Game.game.mainCamera.camera.WorldToScreenPoint(target.transform.position) + screenOffset;
        imageFill.fillAmount = target.health / target.maxHealth;

        if(showTimer > 0){
            showTimer -= Time.deltaTime;
            imageFill.color = Color.white;
            bgImage.color = bgImageColor;
        }else{
            imageFill.color = new Color(0f, 0f, 0f, 0f);
            bgImage.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
