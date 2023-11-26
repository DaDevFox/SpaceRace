using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActivator : MonoBehaviour
{
    public Game.Stage stage;

    void Awake(){
        Game.stageChange += OnFocusChange;
    }

    public void OnFocusChange(Game.Stage stage){
        if(stage == this.stage){
            for(int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(true);
                transform.gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
            }
        }else{
            for(int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
