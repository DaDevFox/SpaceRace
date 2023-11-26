using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MCQController : MonoBehaviour
{   
    public static string indicatorPrefabPath { get; } = "UI/Indicator";
    
    public static event Action questionComplete;

    public static Color hardColor = Color.red;
    public static Color mediumColor = Color.yellow;
    public static Color easyColor = Color.green;


    [Header("Data")]
    public Question question;
    public int playerTurn = 0;
    private int[] playerAnswers = new int[4];

    [Header("Scene")]
    public TextMeshProUGUI questionHeader;
    public TextMeshProUGUI questionText;
    public Image questionTextImage;

    public TextMeshProUGUI[] optionTexts;
    public Image[] optionTextImages;

    public Button[] optionButtons;
    public Transform[] indicators;

    public GameObject[] showDuringQ;
    public GameObject[] showAfterQ;

    public TextMeshProUGUI winText;
    

    [Header("Settings")]
    public int maxTextImgHeight;
    public int maxOptionImgHeight;

    public float showAnswersTime = 3f;
    public float postQLingerTime = 2f;

    public bool blindAnswers = true;

    public static int creditMultiplier = 2;

    public static Func<Boolean>[] optionTriggerTests = new Func<Boolean>[] {
        (() => MainInputController.OneDown || MainInputController.JoystickADown),
        (() => MainInputController.TwoDown || MainInputController.JoystickBDown),
        (() => MainInputController.ThreeDown || MainInputController.JoystickXDown),
        (() => MainInputController.FourDown || MainInputController.JoystickYDown)
    };

    public void Start(){
        ResetQuestion();
    }

    void OnEnable() => Start();

    public void Update(){
        for(int i = 0; i < optionTriggerTests.Length; i++){
            if(optionTriggerTests[i]()){
                ButtonClick(i);
                break;
            }
        }
    }

    public void ButtonClick(int id){
        if(playerAnswers[playerTurn] != -1)
            return;
        
        if(question.options[id].pointValue > 0){
            Game.game.players[playerTurn].currency += question.options[id].pointValue * creditMultiplier * (question.difficulty + 1);
        }

        // if (!blindAnswers)
        // {
        //     indicators[id].GetComponent<Image>().color = Game.game.players[playerTurn].color;
        // }

        playerAnswers[playerTurn] = id;

        playerTurn++;
        playerTurn %= Game.game.players.Length;

        if (CheckPlayerStillNeedsToAnswer())
        {
            Game.game.SetCurrPlayer(playerTurn);

            foreach(GameObject obj in showDuringQ){
                obj.SetActive(true);
            }
            foreach(GameObject obj in showAfterQ){
                obj.SetActive(false);
            }
        }else{
            for(int i = 0; i < Game.game.players.Length; i++){
                indicators[i].GetComponent<Image>().color = question.options[i].pointValue > 0 ? Color.green : Color.red;
            }

            StartCoroutine("LingerCoroutine");
        }
    }

    public IEnumerator LingerCoroutine(){
        
        yield return new WaitForSecondsRealtime(showAnswersTime);
            
        foreach(GameObject obj in showAfterQ){
            obj.SetActive(true);
        }
        foreach(GameObject obj in showDuringQ){
            obj.SetActive(false);
        }
        UpdateWinText();

        yield return new WaitForSecondsRealtime(postQLingerTime);
        ResetQuestion();
        questionComplete?.Invoke();
    }

    private void UpdateWinText(){
        string text = "";

        for(int i = 0; i < playerAnswers.Length; i++){
            if(question.options[playerAnswers[i]].pointValue > 0)
                text += $"<color=#{ColorUtility.ToHtmlStringRGB(Game.game.players[i].color)}>{Game.game.players[i].name}</color>: +<b>{question.options[playerAnswers[i]].pointValue * creditMultiplier * (question.difficulty + 1)}</b> credit{(question.options[playerAnswers[i]].pointValue * creditMultiplier != 1 ? "s" : "")} earned\n";
            if(question.options[playerAnswers[i]].pointValue == 0)
                text += $"<color=#{ColorUtility.ToHtmlStringRGB(Game.game.players[i].color)}>{Game.game.players[i].name}</color>: earned nothing\n";
        }

        winText.text = text;
    }

    public void ResetQuestion(){
        playerTurn = 0;
        Game.game.SetCurrPlayer(0);
        ClearAnswers();

        questionHeader.text = $"Question <b>{question.id}</b> ({(question.difficulty == 0 ? "Easy" : (question.difficulty == 1 ? "Medium" : "Hard"))})";

        if(question.questionText != ""){
            questionText.text = question.questionText;
            questionText.gameObject.SetActive(true);
            questionTextImage.gameObject.SetActive(false);
        }else
        {
            questionText.gameObject.SetActive(false);
            questionTextImage.gameObject.SetActive(true);
            questionTextImage.sprite = question.questionTextImage;
        }

        for(int i = 0; i < 4; i++)
        {
            if(question.options[i].text != ""){
                optionTexts[i].gameObject.SetActive(true);
                optionTextImages[i].gameObject.SetActive(false);
                optionTexts[i].text = question.options[i].text;
            }else{
                optionTexts[i].gameObject.SetActive(false);
                optionTextImages[i].gameObject.SetActive(true);
                Texture2D tex = Resize(question.options[i].textImage.texture, (int)(((float)maxOptionImgHeight/(float)question.options[i].textImage.texture.height) * (float)question.options[i].textImage.texture.width), maxOptionImgHeight);
                optionTextImages[i].sprite = Sprite.Create(tex, new Rect(0,0, (int)(((float)maxOptionImgHeight/(float)question.options[i].textImage.texture.height) * (float)question.options[i].textImage.texture.width), maxOptionImgHeight), Vector2.zero);
            }
        }

        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i].GetComponent<Image>().color = Color.white;
        }

        foreach(GameObject obj in showDuringQ){
            obj.SetActive(true);
        }
        foreach(GameObject obj in showAfterQ){
            obj.SetActive(false);
        }

    }

    public void ClearAnswers(){
        playerAnswers = new int[Game.game.players.Length];
        for (int i = 0; i < playerAnswers.Length; i++)
            playerAnswers[i] = -1;
    }

    private bool CheckPlayerStillNeedsToAnswer()
    {
        bool flag = false;
        for (int i = 0; i < Game.game.players.Length; i++)
        {
            if (playerAnswers[i] == -1)
                flag = true;
        }
        return flag;
    }

    Texture2D Resize(Texture2D texture2D,int targetX,int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY,24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D,rt);
        Texture2D result=new Texture2D(targetX,targetY);
        result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
        result.Apply();
        return result;
    }

}
