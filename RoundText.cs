using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundText : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<TextMeshProUGUI>().text = $"Round {Game.game.round}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
