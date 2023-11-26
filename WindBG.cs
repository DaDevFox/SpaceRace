using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBG : MonoBehaviour
{
    public Texture2D windTex;

    public int scale = 200;

    public static float WindNoise(float x, float y){
        return Mathf.PerlinNoise(x / 5f, y / 5f) - 0.5f;
    }

    void Start()
    {
        windTex = new Texture2D(scale, scale);
        for(int x = 0; x < scale; x++){
            for(int y = 0; y < scale; y++)
            {
                float val = WindNoise((float)x, (float)y) + 0.5f;
                Color color = new Color(val, val, val);
                windTex.SetPixel(x, y, color);
            }
        }
        windTex.Apply();

        GetComponent<MeshRenderer>().material.mainTexture = windTex;
    }
}
