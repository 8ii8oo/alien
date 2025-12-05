using System;
using UnityEngine;

public class leftRight : MonoBehaviour
{
    public float moveSpeed;
    public float amplitude;
    Vector2 startPosition;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        float newX = startPosition.x + amplitude * (float)Math.Sin(Time.time * moveSpeed);
        
        rb.MovePosition(new Vector2(newX, startPosition.y));
    }

    void Update()
    {
        
    }
}