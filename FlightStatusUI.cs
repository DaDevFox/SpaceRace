using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlightStatusUI : MonoBehaviour
{
    public TextMeshProUGUI[] positionTexts;

    public List<Rocket> rockets;

    public Button launchButton;

    public float significance = 1f;
    
    public float postLaunchLingerTime = 2f;

    private float timer = 0f;

    public EndOfLaunchUI endscreen;
    public GameObject[] deactivateOnEnd;

    void Start(){
        rockets = new List<Rocket>();
        for(int i = 0; i < Game.game.players.Length; i++){
            rockets.Add(Game.game.players[i].launcher.rocket);
        }
        // endLaunchButton.gameObject.SetActive(false);
    }

    void OnEnable() => Start();

    public void Activate() => Start();

    void Update()
    {
        rockets = rockets.OrderByDescending((rocket) => rocket.maxHeight).ToList();

        bool endLaunchAvailable = true;
        for(int i = positionTexts.Length - 1; i >= 0; i--){
            Player player = Game.game.players[rockets[i].launcher.player];

            string text = $"{(rockets[i].controlledFlight ? "" : "<b>X</b> ")}<color=#{ColorUtility.ToHtmlStringRGB(player.color)}>{player.name}</color>: {Numerify(rockets[i].maxHeight/10f, 3)} km";

            positionTexts[i].text = text;

            if(Game.prelaunch || (rockets[i].controlledFlight || (rockets[i].rigidbody.velocity.x > significance || rockets[i].rigidbody.velocity.y > significance)))
                endLaunchAvailable = false;
        }

        if(endLaunchAvailable){
            timer += Time.deltaTime;
            if(timer > postLaunchLingerTime){
                endscreen.Display();
                foreach(GameObject obj in deactivateOnEnd)
                    obj.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        else{
            // endLaunchButton.interactable = false;
            timer = 0f;
        }
    }

    public static string Numerify(float value, int preDecFigures){
        return value.ToString($"#00.0");
    }
}
