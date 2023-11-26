using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInputController : MonoBehaviour
{
    public static bool debug {get;} = true;

    public static bool ShiftHeld => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

    public static bool SpaceHeld => Input.GetKey(KeyCode.Space) || JoystickAHeld;
    public static bool SpaceDown => Input.GetKeyDown(KeyCode.Space) || JoystickADown;
    public static bool SpaceUp => Input.GetKeyUp(KeyCode.Space) || JoystickAUp;
    
    public static bool ZeroHeld => Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Keypad0);
    public static bool ZeroDown => Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0);
    public static bool ZeroUp => Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0);

    public static bool OneHeld => Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1);
    public static bool OneDown => Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1);
    public static bool OneUp => Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1);    

    public static bool JoystickAHeld => Input.GetKey(KeyCode.JoystickButton0);
    public static bool JoystickADown => Input.GetKeyDown(KeyCode.JoystickButton0);
    public static bool JoystickAUp => Input.GetKeyUp(KeyCode.JoystickButton0);
    
    public static bool TwoHeld => Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2);
    public static bool TwoDown => Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2);
    public static bool TwoUp => Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2);

    public static bool JoystickBHeld => Input.GetKey(KeyCode.JoystickButton1);
    public static bool JoystickBDown => Input.GetKeyDown(KeyCode.JoystickButton1);
    public static bool JoystickBUp => Input.GetKeyUp(KeyCode.JoystickButton1);
    
    public static bool ThreeHeld => Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3);
    public static bool ThreeDown => Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3);
    public static bool ThreeUp => Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3);

    public static bool JoystickXHeld => Input.GetKey(KeyCode.JoystickButton2);
    public static bool JoystickXDown => Input.GetKeyDown(KeyCode.JoystickButton2);
    public static bool JoystickXUp => Input.GetKeyUp(KeyCode.JoystickButton2);
    
    public static bool FourHeld => Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4);
    public static bool FourDown => Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4);
    public static bool FourUp => Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4);
    
    public static bool JoystickYHeld => Input.GetKey(KeyCode.JoystickButton3);
    public static bool JoystickYDown => Input.GetKeyDown(KeyCode.JoystickButton3);
    public static bool JoystickYUp => Input.GetKeyUp(KeyCode.JoystickButton3);

    public static bool PlusDown => Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus);
    public static bool MinusDown => Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus);

    public static bool FireDown => Input.GetMouseButtonDown(0);

    
    void Update()
    {
        if(debug){
            // if(ZeroDown)
            //     Game.game.focusingPlayer = -1;

            // else if(ShiftHeld && OneDown)
            //     Game.game.SetCurrPlayer(0);
            // else if(OneDown)
            //     Game.game.focusingPlayer = 0;
                
            // else if(ShiftHeld && TwoDown)
            //     Game.game.SetCurrPlayer(1);
            // else if(TwoDown)
            //     Game.game.focusingPlayer = 1;

            // else if(ShiftHeld && ThreeDown)
            //     Game.game.SetCurrPlayer(2);
            // else if(ThreeDown)
            //     Game.game.focusingPlayer = 2;

            // else if(ShiftHeld && FourDown)
            //     Game.game.SetCurrPlayer(3);
            // else if(FourDown)
            //     Game.game.focusingPlayer = 3;

            // if(PlusDown)
            //     TurnSystem.Increment();
            // else if (MinusDown)
            //     TurnSystem.Increment(false);
        }
    }
}
