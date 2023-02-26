using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WeightLifter : MonoBehaviour
{
    public float MaxPoints = 910;
    public float Points = 0;
    private float CurrentTime;
    private float startTime = 10f;

    private SpriteRenderer sr;

    public Sprite repUp;
    public Sprite repDown;

    private bool isCountingDown = true;
    private bool isGameOver;
    public bool isrepUp;
    public Animator anim;

    public TextMeshProUGUI cootsScore;

    void Start()
    {
        CurrentTime = startTime;
        isGameOver = false;
        isrepUp = false;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Points >= 910 || !isCountingDown)
        {
            isGameOver = true;
        }

        if (isCountingDown == true)
        {
            CurrentTime -= 1 * Time.deltaTime;
            //countdown
        }

        if (CurrentTime <= 0)
        {
            CurrentTime = 0;
            isCountingDown = false;
            Debug.Log("done");
            //stops the countdown
        }

        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Points = (Points + 10);
                if (Points == 910)
                {
                    Points = 910;
                }
                Debug.Log(Points);
                if (isrepUp)
                {
                    sr.sprite = repUp;
                    isrepUp = false;
                }
                else if(!isrepUp)
                {
                    sr.sprite = repDown;
                    isrepUp = true;
                }
            }
            



        }
         else if (Points < 910 && isGameOver)
            {
            Debug.Log("You Lose");
        }




        if (Points >= 910)
        {
            Debug.Log("You Win");
        }

        cootsScore.text = Points.ToString();
        





    }
}
