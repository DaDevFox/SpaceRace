using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnText : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Awake()
    {
        Game.playerChange += PlayerChange;
        Game.focusChange += FocusChange;   
    }

    void Start(){
        PlayerChange(Game.game.currPlayer.id);
    }

    public void PlayerChange(int player){
        UpdateText();
    }
    public void FocusChange(int player){
        UpdateText();
    }

    public void UpdateText(){
        text.text = $"<color=#{ColorUtility.ToHtmlStringRGB(Game.game.currPlayer.color)}>{Game.game.currPlayer.name}</color>";
        if(Game.game.focusingPlayer != -1)
            text.text += $" vs. <color=#{ColorUtility.ToHtmlStringRGB(Game.game.players[Game.game.focusingPlayer].color)}>{Game.game.players[Game.game.focusingPlayer].name}</color>";
    }
}
