using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitOnClick : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => Application.Quit());
    }
}
