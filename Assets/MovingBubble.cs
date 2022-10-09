using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBubble : MonoBehaviour
{
    public Rigidbody2D rb;
    internal void Fire(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
