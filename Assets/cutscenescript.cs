using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cutscenescript : MonoBehaviour
{
    void Update()
    {
        SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
    }

}
