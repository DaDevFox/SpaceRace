using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorSwitcher : MonoBehaviour
{

    public Color lightImage = Color.white;
    public Color darkImage = Color.black;

    public Color lightText = Color.black;
    public Color darkText = Color.white;
    
    public bool invert;
    
    void Start()
    {
        if(GetComponent<Image>())
            GetComponent<Image>().color = Game.uiMode == Game.UIMode.DARK ? (invert ? lightImage : darkImage) : (invert ? darkImage : lightImage);
        if(GetComponent<TextMeshProUGUI>())
            GetComponent<TextMeshProUGUI>().color = Game.uiMode == Game.UIMode.DARK ? (invert ? lightText : darkText) : (invert ? darkText : lightText);
    }
}
