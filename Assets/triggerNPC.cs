using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerNPC : MonoBehaviour
{
    public GameObject inputPlayer;
    public GameObject dialoguePanel;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        dialoguePanel.SetActive(true);
        NPC.instance.textStart = true;
        // collision.gameObject.GetComponent<PlayerController>().canMove = false;
        inputPlayer.SetActive(false);
        GetComponent<CapsuleCollider2D>().isTrigger = true;
    }
    
}
