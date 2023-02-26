using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerScript : MonoBehaviour
{
    public string nextScene;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene(nextScene);
    }

}
