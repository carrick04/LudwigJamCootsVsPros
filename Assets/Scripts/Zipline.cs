using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 10f;

    void Start()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = (endPoint.position - startPoint.position).normalized * speed;
        }
    }
}
