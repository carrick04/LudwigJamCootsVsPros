using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HobScript : MonoBehaviour
{
    public float timer;
    private float countdown;
    private bool hobIsActive;
    public GameObject fire;

    void Start()
    {
        countdown = timer;
        hobIsActive = false;
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0f)
        {
            countdown = timer;
            
            if (hobIsActive)
            {
                fire.SetActive(true);
                hobIsActive = false;
            }
            else
            {
                hobIsActive = true;
                fire.SetActive(false);
            }
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().KillPlayer();
        }
    }
    */
}
