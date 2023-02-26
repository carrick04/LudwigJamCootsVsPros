using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TextClimb : MonoBehaviour
{
    public float speed = 1f;
    
    public Transform[] points;
    public float lerpSpeed = 5f;
    public float delay = 1.0f;

    public int currentPointIndex = 0;
    public bool isLerping = false;
    private float lerpStartTime;

    public GameObject restartUI;
    public TMPro.TMP_Text rsText;
    public SpeechScript speechScript = new SpeechScript();
    private bool textWritten = false;

    private void Awake()
    {
        
    }

    void Start()
    {      
        transform.position = points[currentPointIndex].position;
        Invoke("StartLerping", delay);
    }

    void Update()
    {
        Debug.Log(points.Length + " : " + currentPointIndex);
        if (currentPointIndex == points.Length -1)
        {
            //Debug.Log("Should set the fuking thing active now");
            //restartUI.SetActive(true);
            //if (!textWritten)
            //{
            //    speechScript.AddWriter(rsText, "I'm sure you'll do beter next time", 0.1f, true);
            //    textWritten = true;
            //}
            //PlayerClimb.instance.magnusWon = true;
            //return;
            SceneManager.LoadScene("THOR WIN COOTS");
        }
        
        
        if (isLerping)
        {
            float timeSinceLerpStart = Time.time - lerpStartTime;
            float lerpPercentComplete = timeSinceLerpStart / (Vector3.Distance(points[currentPointIndex].position, points[currentPointIndex + 1].position) / speed);

            transform.position = Vector3.Lerp(points[currentPointIndex].position, points[currentPointIndex + 1].position, lerpPercentComplete);

            
            if (lerpPercentComplete >= 1.0f)
            {
                currentPointIndex++;

                if (currentPointIndex < points.Length - 1)
                {
                    isLerping = false;                   
                    Invoke("StartLerping", delay);
                }
            }

            if (currentPointIndex == points.Length)
            {
                //Win the game
                Debug.Log("Magnus Win");
            }

        }
    }

    private void StartLerping()
    {
        isLerping = true;
        lerpStartTime = Time.time;
    }

    
}
