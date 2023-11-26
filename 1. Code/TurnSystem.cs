using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static int currPlayer = 0;

    public static void Increment(bool up = true){
        if(up)
            currPlayer ++;
        else{
            currPlayer--;
            if(currPlayer < 0)
                currPlayer = Game.game.numPlayers - 1;
        }
        currPlayer %= Game.game.numPlayers;
        Game.NotifyPlayerChange(currPlayer);
    }
}
