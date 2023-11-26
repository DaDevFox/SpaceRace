using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCounter : MonoBehaviour
{
    
    public TextAsset file;

    void Start()
    {
        string text = file.text;
        Debug.Log(text.Split(' ').Length);
    }
}
