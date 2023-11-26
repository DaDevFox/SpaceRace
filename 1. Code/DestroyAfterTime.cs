using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public bool scaled = true;
    public float time = 1f;

    private float elapsed = 0f;

    void Update()
    {
        elapsed += scaled ? Time.deltaTime : Time.unscaledDeltaTime;

        if(elapsed > time)
            GameObject.Destroy(gameObject);
        
    }
}
