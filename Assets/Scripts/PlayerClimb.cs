using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerClimb : MonoBehaviour
{
    public static PlayerClimb instance;
    public float speed = 1f;
    public TMPro.TMP_Text nextKeyText;

    private KeyCode nextKey;
    private bool nextKeypressed;

    public Transform[] points;
    public float lerpSpeed = 5f;
    public float delay = 1.0f;

    public int currentPointIndex = 0;
    public bool isLerping = false;
    private float lerpStartTime;
    public bool magnusWon;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Generate a random key from A-Z
        nextKey = (KeyCode)Random.Range((int)KeyCode.A, (int)KeyCode.Z + 1);
        nextKeypressed = false;
        nextKeyText.text = nextKey.ToString();
        transform.position = points[currentPointIndex].position;
        magnusWon = false;
    }

    void Update()
    {
        nextKeyText.transform.position = PlayerClimb.instance.points[PlayerClimb.instance.currentPointIndex + 1].position;
        if (magnusWon)
        {
            return;
        }

        if (currentPointIndex == points.Length - 2 || currentPointIndex == points.Length - 1) 
        {
            SceneManager.LoadScene("COOTS WIN v MAGNUS");
            return;
        }
        

        if (!isLerping)
        {
            if (Input.GetKeyDown(nextKey))
            {
                nextKeypressed = true;
                isLerping = true;
                lerpStartTime = Time.time;
            }
        }
        
        
        if (isLerping)
        {
            float timeSinceLerpStart = Time.time - lerpStartTime;
            float lerpPercentComplete = timeSinceLerpStart / (Vector3.Distance(points[currentPointIndex].position, points[currentPointIndex + 1].position) / speed);

            transform.position = Vector3.Lerp(points[currentPointIndex].position, points[currentPointIndex + 1].position, lerpPercentComplete);

            nextKeyText.text = " ";
            if (lerpPercentComplete >= 1.0f && nextKeyText.text == " ")
            {
                currentPointIndex++;

                if (currentPointIndex < points.Length - 1)
                {
                    isLerping = false;
                    nextKeyText.text = nextKey.ToString();
                }
            }

            
        }

        if (nextKeypressed)
        {
            nextKeypressed = false;
            nextKey = (KeyCode)Random.Range((int)KeyCode.A, (int)KeyCode.Z + 1);
            nextKeyText.text = nextKey.ToString();
        }

    }
    public void RestartScene()
    {
        SceneManager.LoadScene("Rock Climbing Game");
    }
}
