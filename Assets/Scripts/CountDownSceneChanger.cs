using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownSceneChanger : MonoBehaviour
{
    public string NextLevel;
    public float timer = 0.37f;


    void Start()
    {
        
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Application.LoadLevel(NextLevel);
        }
        
    }
}
