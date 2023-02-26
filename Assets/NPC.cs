using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    private int index = 0;
    public float typingSpeed;

    public GameObject contuinueButton;
    public static NPC instance;
    public bool textStart;
    public string sceneToLoad;

    private void Start()
    {
        instance = this;
        textStart = false;
        
    }

    void Update()
    {
        if(dialogueText.text == dialogueLines[index])
        {
            contuinueButton.SetActive(true);
        }
        if (textStart)
        {
            StartCoroutine(Type());
            textStart = false;
        }
    }

    IEnumerator Type()
    {
        foreach(char letter in dialogueLines[index].ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void nextSentence()
    {
        contuinueButton.SetActive(false);
        if(index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Type());

        }
        else
        {
            dialogueText.text = "";
            contuinueButton.SetActive(false);
            SceneManager.LoadScene(sceneToLoad);
        }

    }

    


}

