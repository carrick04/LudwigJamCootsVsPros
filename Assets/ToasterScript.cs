using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterScript : MonoBehaviour
{
    //public Animator animator;
    public GameObject toaster;
    public Sprite toastUp;
    private void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 850f));
        toaster.GetComponent<SpriteRenderer>().sprite = toastUp;
    }
}
