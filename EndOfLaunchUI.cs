using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndOfLaunchUI : MonoBehaviour
{
    public Image[] fade;

    public TextMeshProUGUI statsText;

    Dictionary<float, string> incentiveText = new Dictionary<float, string>(){
        {20f, "The first player(s) to get to 20 km will earn one additional credit"},
        {40f, "The first player(s) to get to 40 km will earn two additional credit"},
        {60f, "The first player(s) to get to 60 km will earn two additional credits"},
        {80f, "The first player(s) to get to 80 km will earn two additional credits"},
        {100f, "Reach the Karman line (100km) to win!"}
    };

    public static Dictionary<int, int> creditBonuses = new Dictionary<int, int>(){
        {200, 1},
        {400, 2},
        {600, 2},
        {800, 2},  
    };

    private Dictionary<float, string> failureTexts = new Dictionary<float, string>(){
        {1f, "Useless."},
        {1.1f, "I have no words."},
        {1.2f, "Well that was a flop"},
        {12f, "Go back to med school or something."},
        {13f, "Don't you have better things to do?"},
        {25f, "Fun, but it seems like a lot of effort to go about 30 km"},
        {35f, "Certainly better than last time."},
        {40f, "Huh, didn't think you'd get that far"},
        {41f, "Getting there ig"},
        
        {60f, "..."},
        {90f, "So close yet so far"},

        {100f, "Dang you made it."}




    };

    private List<string> usedFailureTexts = new List<string>();


    public float fadeInTime = 1f;
    public float readTime = 5f;
    public float lingerTime = 3f;

    public bool showFailureTexts = true;

    public static int CalcBonusCredits(float maxHeight){
        List<int> bonuses = new List<int>();
        foreach(float benchmark in creditBonuses.Keys){
            if(maxHeight > benchmark && Game.bonusesUnclaimed.Contains((int)benchmark)){
                bonuses.Add((int)benchmark);
            }
        }

        int sumCredits = 0;
        foreach(int bonus in bonuses)
            if(creditBonuses.ContainsKey(bonus))
                sumCredits += creditBonuses[bonus];
        return sumCredits;
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        foreach(Image image in fade)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        statsText.text = "";
    }

    void Start() {
        OnEnable();
    }

    public void Display(){
        StartCoroutine("LingerCoroutine");
    }

    public void UpdateText(){
        foreach(Image image in fade)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

        string text = $"Round {Game.game.round}: <b>End of Round Summary</b>\n\n";

        float maxTotal = 0f;

        for(int i = 0; i < Game.game.players.Length; i++){
            float maxHeight = Game.game.players[i].launcher.rocket.maxHeight;
            if(maxHeight > maxTotal)
                maxTotal = maxHeight;
        }

        Game.gameVictor = 0;
        for(int i = 0; i < Game.game.players.Length; i++)
            if(Game.game.players[i].launcher.rocket.maxHeightTotal > Game.game.players[Game.gameVictor].launcher.rocket.maxHeightTotal)
                Game.gameVictor = i;

        if(maxTotal > Game.winHeight){
            Player victor = Game.game.players[Game.gameVictor];
            text += $"<b><color=#{ColorUtility.ToHtmlStringRGB(victor.color)}>{victor.name}</color> won!</b>\n";

            foreach(Player player in Game.game.players)
                if(player != victor)
                    text += $"<color=#{ColorUtility.ToHtmlStringRGB(player.color)}>{player.name}</color> flew {Mathf.Floor(player.launcher.rocket.maxHeight/10f)} km\n";
        }else{
            for(int i = 0; i < Game.game.players.Length; i++)
            {
                float maxHeight = Game.game.players[i].launcher.rocket.maxHeight;
                Player player = Game.game.players[i];            
                
                text += $"<color=#{ColorUtility.ToHtmlStringRGB(player.color)}>{player.name}</color> flew {Mathf.Floor(maxHeight/10f)} km";
                if(player.launcher.rocket.bonusCount > 0){
                    text += $" (<b>{player.launcher.rocket.bonusCount}</b> bonus credit{(player.launcher.rocket.bonusCount != 1 ? "s" : "")} earned)";
                }
                text += "\n";
            }
        }

        text += "\n";

        string failureTextChosen = "";
        foreach(float benchmark in failureTexts.Keys){
            if(maxTotal > benchmark*10f && !usedFailureTexts.Contains(failureTexts[benchmark])){
                failureTextChosen = failureTexts[benchmark];
            }
            if(maxTotal < benchmark*10f)
                break;
        }

        if(showFailureTexts)
            text += failureTextChosen + "\n";

        text += "\n";

        string incentiveTextChosen = "Good luck passing the karman line!";
        foreach(float benchmark in incentiveText.Keys){
            if(maxTotal < benchmark*10f){
                incentiveTextChosen = incentiveText[benchmark];
                break;
            }
        }


        text += $"<i>{incentiveTextChosen}</i>\n";

        statsText.text = text;
    }

    public IEnumerator LingerCoroutine(){
        float timer = 0f;
        while(timer < fadeInTime){
            foreach(Image image in fade)
                image.color = new Color(image.color.r, image.color.g, image.color.b, timer/fadeInTime);
            
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        for(int i = 0; i < Game.game.players.Length; i++){
            Rocket rocket = Game.game.players[i].launcher.rocket;
            if(rocket.maxHeight > rocket.maxHeightTotal)
                rocket.maxHeightTotal = rocket.maxHeight;
        }
        
        foreach(Image image in fade)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        UpdateText();            

        yield return new WaitForSecondsRealtime(readTime);
        
        float maxTotal = 0f;

        for(int i = 0; i < Game.game.players.Length; i++){
            float maxHeight = Game.game.players[i].launcher.rocket.maxHeight;
            if(maxHeight > maxTotal)
                maxTotal = maxHeight;
        }

        for(int i = 0; i <= lingerTime; i++)
        {
            UpdateText();
            
            if(maxTotal <= Game.winHeight)
                statsText.text += $"\n\nBeginning Round {Game.game.round + 1} in {lingerTime - i}...";
            
            yield return new WaitForSecondsRealtime(1f);
        }

        string failureTextChosen = "";
        foreach(float benchmark in failureTexts.Keys){
            if(maxTotal > benchmark && !usedFailureTexts.Contains(failureTexts[benchmark])){
                failureTextChosen = failureTexts[benchmark];
                break;
            }
        }
        
        usedFailureTexts.Add(failureTextChosen);

        if(maxTotal > Game.winHeight)
        {
            Game.stage = Game.Stage.VICTORY;
        }else{
            Game.game.round++;
            Game.stage = Game.Stage.MCQ;
        }
    }
}
