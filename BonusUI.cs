using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusUI : MonoBehaviour
{
    [Header("Settings")]
    public int player;
    public Vector3 location;
    public int amount;
    public float honeInTime = 1f;
    public float widthStart = 200f;
    public float widthTarget = 100f;

    
    public float lingerTime = 1f;
    public float fadeOutTime = 1f;

    public AnimationCurve fade;
    
    [Header("Scene")]
    public TextMeshProUGUI bonusNumText;


    private float time = 0f;

    public static string prefabPath {get;} = "2. Prefabs/BonusAqcuiredUI"; 

    public static void Create(Vector3 location, int player, int amount){
        BonusUI instance = GameObject.Instantiate(Resources.Load<GameObject>(prefabPath), Game.game.mainCamera.camera.WorldToScreenPoint(location), Quaternion.identity).GetComponent<BonusUI>();
        instance.location = location;
        instance.player = player;
        instance.amount = amount;
        instance.transform.SetParent(Game.game.canvas.transform);
    }

    void Start()
    {
        bonusNumText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(Game.game.players[player].color)}>+{amount.ToString()}</color>";
        StartCoroutine("FadeCoroutine");
    }

    public IEnumerator FadeCoroutine(){
        Image image = GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 1f);
        bonusNumText.color = new Color(1f, 1f, 1f, 1f);
            
        while(time < honeInTime){
            ((RectTransform)transform).sizeDelta = new Vector2(widthStart - (widthStart - widthTarget) * (time/honeInTime), widthStart - (widthStart - widthTarget) * (time/honeInTime));
            
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        time = 0f;
        yield return new WaitForSecondsRealtime(lingerTime);

        while(time < honeInTime){
            image.color = new Color(1f, 1f, 1f, 1f - time/honeInTime);
            bonusNumText.color = new Color(1f, 1f, 1f, 1f - time/honeInTime);
            
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        GameObject.Destroy(gameObject);
    }

    void Update()
    {
        // if(time < honeInTime){
        //     ((RectTransform)transform).sizeDelta = new Vector2(widthStart - (widthStart - widthTarget) * (time/honeInTime), widthStart - (widthStart - widthTarget) * (time/honeInTime));
            
        //     Image image = GetComponent<Image>();
        //     image.color = new Color(1f, 1f, 1f, 1f - time/honeInTime);
        //     bonusNumText.color = new Color(1f, 1f, 1f, 1f - time/honeInTime);
            
        //     time += Time.deltaTime;
        // }else{

        // }

        transform.position = Game.game.mainCamera.camera.WorldToScreenPoint(location);
    }
}
