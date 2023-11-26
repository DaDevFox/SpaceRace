using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    public int player;

    private TMP_InputField input;

    void Start()
    {
        input = GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener((name) => {Game.game.players[player].name = name.Trim() != "" ? name : $"Player {player + 1}";});
    }
}
