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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bubble") == false)
            return;

        //멈춰진 버블 생성하자.
        Bubble bubble = GetComponentInChildren<Bubble>(true);
        if (bubble == null)
            return;
        bubble.col.enabled = true;
        TilemapManager.instance.AddBubble(bubble);

        //움직이는버블 부스고,
        Destroy(gameObject);
    }
}
