using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ThorLiftz : MonoBehaviour
{
    int MaxPoints;
    public int points = 0;
    private float CurrentTime;
    private float startTime = 10f;

    private SpriteRenderer sr;

    public Sprite repUp;
    public Sprite repDown;

    private bool isCountingDown = true;
    private bool gameOver;
    public bool isRepUp;

    public TextMeshProUGUI thorScore;

    void Start()
    {
        CurrentTime = startTime;
        gameOver = false;
        isRepUp = false;
        sr = GetComponent<SpriteRenderer>();
        MaxPoints = UnityEngine.Random.Range(901, 909);
        Debug.Log(MaxPoints);
        points = 0;
        StartCoroutine(AddPoints());
    }

    IEnumerator AddPoints()
    {
        while (points < MaxPoints)
        {
            points += 10;
            thorScore.text = points.ToString();

            if (points % 10 == 0)
            {
                isRepUp = !isRepUp;
                sr.sprite = isRepUp ? repUp : repDown;
            }

            yield return new WaitForSeconds(0.1f);
        }

        if (points > MaxPoints)
        {
            points = MaxPoints;
            thorScore.text = points.ToString();
        }

        Debug.Log("Win");
        SceneManager.LoadScene("THOR WIN");
    }

    void Update()
    {

    
    }

   


}
