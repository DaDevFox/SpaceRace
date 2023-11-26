using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObj : MonoBehaviour
{
    public float maxHealth = 1f;
    [SerializeField]
    private float _health = 1f;
    public float health{
        get => _health;
        set {
            if(_health != value){
                _health = value;
                onHealthChange?.Invoke();
            }
        }
    }

    public event Action onHealthChange;

    public static string healthBarPrefabPath {get;} = "2. Prefabs/Healthbar";
    private GameObject healthbarPrefab;
    private GameObject healthbar;
    
    // Start is called before the first frame update
    void Start()
    {
        healthbarPrefab = Resources.Load<GameObject>(healthBarPrefabPath);
        GameObject instance = GameObject.Instantiate(healthbarPrefab, Game.game.canvas.transform);
        
        instance.GetComponent<Healthbar>().target = this;
        healthbar = instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(health < 0f){
            GameObject.Destroy(gameObject);
            GameObject.Destroy(healthbar);
        }
    }
}
