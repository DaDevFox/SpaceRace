using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI statsText;

    void Update()
    {
        if(Game.gameVictor != -1){
            headerText.text = $"{Game.game.players[Game.gameVictor].name} won!";
            statsText.text = $"Maximum rocket height: {Mathf.Round(Game.game.players[Game.gameVictor].launcher.rocket.maxHeightTotal/10f)}km";
        }
    }
}
