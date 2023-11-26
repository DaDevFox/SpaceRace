using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FocusSelector : MonoBehaviour
{
    public RectTransform manipulable;
    public Rocket selectable;
    public Image buttonImage;

    public Image fuelFill;

    public GameObject winningImg;

    public TextMeshProUGUI playerText;
    
    void Start(){
    }

    public void OnClick(){
        // Game.game.focusingPlayer = selectable.launcher.player;
    }

    void Update()
    {
        Vector3 screenPos = Game.game.mainCamera.camera.WorldToScreenPoint(selectable.transform.position);
        manipulable.position = screenPos;
        buttonImage.color = Game.game.players[selectable.launcher.player].color;

        playerText.text = Game.game.players[selectable.launcher.player].name;
        fuelFill.fillAmount = selectable.fuel / selectable.maxFuel;

        if(selectable.maxHeight > Game.winHeight){
            winningImg.SetActive(true);
        }
        else
            winningImg.SetActive(false);
    }
}
