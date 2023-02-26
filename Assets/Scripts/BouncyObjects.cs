using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyObjects : MonoBehaviour
{
    public float bounceForce = 10f;
    private bool isBouncing = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isBouncing)
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 upwardForce = transform.up * bounceForce;
            rb.AddForce(upwardForce, ForceMode2D.Impulse);
            isBouncing = true;
            Invoke("ResetBounce", 0.1f); // Reset isBouncing after 0.1 seconds
        }
    }

    private void ResetBounce()
    {
        isBouncing = false;
    }
}
