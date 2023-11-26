using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWithUpgrade : MonoBehaviour
{
    public Rocket rocket;

    public Upgrades upgrade;

    // public bool moreFuel1;
    // public bool moreFuel2;
    // public bool moreFuel3;
    // public bool moreThrust1;
    // public bool moreThrust2;
    // public bool moreThrust3;
    // public bool moreControl1;
    // public bool moreControl2;
    // public bool radio;

    private bool pastEnabled;

    void OnEnable(){
        UpdateParts();
    }

    void Start() => OnEnable();

    public void UpdateParts()
    {
        // Debug.Log(Game.game.players[rocket.launcher.player].upgrades);
        if((int)(Game.game.players[rocket.launcher.player].upgrades & upgrade) > 0){
            for(int i = 0; i < transform.childCount; i++)
                if(!transform.GetChild(i).gameObject.activeSelf)
                    transform.GetChild(i).gameObject.SetActive(true);
        }
        else{
            for(int j = 0; j < transform.childCount; j++)
                if(transform.GetChild(j).gameObject.activeSelf)
                    transform.GetChild(j).gameObject.SetActive(false);
        }
    }
}
