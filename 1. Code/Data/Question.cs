using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/Question")]
public class Question : ScriptableObject
{
    public int id;

    [TextArea]
    public string questionText;
    public Sprite questionTextImage;

    public Option[] options = new Option[4];

    public int difficulty = 0;




    [Serializable]
    public class Option 
    {
        public Sprite textImage;
        public string text;
        public int pointValue = 0;
    }
}
